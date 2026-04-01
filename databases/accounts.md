# Diseño de Cuentas — Desde Cero

Cada tipo de cuenta tiene su propia tabla. No hay tabla genérica.

---

## Enum LinkedAccountType

```csharp
public enum LinkedAccountType
{
    Email = 1,
    GitHub = 2
}
```

Usado en `AccountKiro` para saber a qué tabla apunta el `RefId`.

---

## Tablas

### AccountEmail
```sql
CREATE TABLE AccountEmail (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Provider NVARCHAR(50) NOT NULL,        -- gmail, outlook, otro
    Email NVARCHAR(200) NOT NULL,
    Password NVARCHAR(200) NOT NULL,
    Phone NVARCHAR(20),
    RecoveryEmail NVARCHAR(200),
    IsNew BIT NOT NULL DEFAULT 1,
    LastUsed DATETIME,
    CreatedAt DATETIME DEFAULT GETDATE()
);
```

---

### AccountSteam
Referencia a `AccountEmail` por `EmailId` — no se escribe el correo.

```sql
CREATE TABLE AccountSteam (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    EmailId INT NOT NULL,                  -- FK a AccountEmail
    Username NVARCHAR(200) NOT NULL,
    Password NVARCHAR(200) NOT NULL,
    Phone NVARCHAR(20),
    ProfileUrl NVARCHAR(1000),
    HasDota2 BIT NOT NULL DEFAULT 0,
    HasCS2 BIT NOT NULL DEFAULT 0,
    IsUnlimited BIT NOT NULL DEFAULT 0,
    IsVacBanned BIT NOT NULL DEFAULT 0,
    IsNew BIT NOT NULL DEFAULT 1,
    LastUsed DATETIME,
    CreatedAt DATETIME DEFAULT GETDATE()
);
```

---

### AccountGeneral
Facebook, Rakion, LOL, Instagram, u otras plataformas. Sin IsNew ni LastUsed.

```sql
CREATE TABLE AccountGeneral (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Platform NVARCHAR(100) NOT NULL,       -- Facebook, Rakion, Instagram, LOL, etc.
    Username NVARCHAR(200) NOT NULL,
    Password NVARCHAR(200) NOT NULL,
    EmailId INT,                           -- FK a AccountEmail (opcional)
    ProfileUrl NVARCHAR(1000),
    CreatedAt DATETIME DEFAULT GETDATE()
);
```

---

### AccountGitHub
Referencia a `AccountEmail` por `EmailId`.

```sql
CREATE TABLE AccountGitHub (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    EmailId INT NOT NULL,                  -- FK a AccountEmail
    Username NVARCHAR(200) NOT NULL,
    Password NVARCHAR(200) NOT NULL,
    ProfileUrl NVARCHAR(1000),
    IsNew BIT NOT NULL DEFAULT 1,
    LastUsed DATETIME,
    CreatedAt DATETIME DEFAULT GETDATE()
);
```

---

### AccountKiro
Una sola cuenta. Sin nombre. Referencia o a un Email o a un GitHub mediante `LinkedType` + `RefId`.
La renovación es automática: día 1 del mes siguiente al `LastUsed`.

```sql
CREATE TABLE AccountKiro (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    LinkedType INT NOT NULL,               -- 1: Email, 2: GitHub (LinkedAccountType)
    RefId INT NOT NULL,                    -- Id de AccountEmail o AccountGitHub según LinkedType
    IsNew BIT NOT NULL DEFAULT 1,
    LastUsed DATETIME,
    CreatedAt DATETIME DEFAULT GETDATE()
);
```

---

## Lógica de IsNew

| IsNew | Significado | LastUsed |
|-------|-------------|----------|
| 1 | Nueva, nunca usada | NULL |
| 0 | Reutilizada | Fecha de último uso (obligatorio) |

Al marcar "usar" → `LastUsed = hoy`, `IsNew = 0`

---

## Renovación Kiro (automática)

```ts
const needsRenewal = (lastUsed: string | null): boolean => {
  if (!lastUsed) return false;
  const last = new Date(lastUsed);
  const renewDate = new Date(last.getFullYear(), last.getMonth() + 1, 1);
  return new Date() >= renewDate;
};
```

---

## Vista — Tabs

| Tab | Tabla |
|-----|-------|
| Email | AccountEmail |
| Steam | AccountSteam |
| General | AccountGeneral |
| GitHub | AccountGitHub |
| Kiro | AccountKiro |

---

## Formularios por tipo

### Email
- Proveedor (gmail / outlook / otro)
- Correo, Contraseña, Celular, Recovery email
- IsNew → si false: fecha último uso

### Steam
- Select de correos existentes → muestra `Email` de `AccountEmail`
- Usuario, Contraseña, Celular, URL perfil
- Checkboxes: HasDota2, HasCS2, IsUnlimited, IsVacBanned
- IsNew → si false: fecha último uso

### General (Facebook, Juegos, Instagram, etc.)
- Plataforma (Facebook, Rakion, Instagram, LOL, etc.)
- Usuario, Contraseña, URL perfil
- Select de correo existente (opcional) → muestra `Email` de `AccountEmail`

### GitHub
- Select de correos existentes → muestra `Email` de `AccountEmail`
- Usuario, Contraseña, URL perfil
- IsNew → si false: fecha último uso

### Kiro
- LinkedType: Email (1) o GitHub (2)
- RefId: select de `AccountEmail` o `AccountGitHub` según LinkedType
  - Si Email: muestra el correo
  - Si GitHub: muestra el username
- IsNew → si false: fecha último uso
- Sin campo de renovación — es automático
