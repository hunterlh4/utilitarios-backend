

-- Usuario 1: Admin
INSERT INTO Users (Username, PasswordHash, SuperUser, UserType, CreatedAt)
VALUES ('admin', '$2a$13$pHywiuK9AY4X/BORNdpNaeINFbvePylHLH.d6NiLEr.lUKWNEbooW', 1, 0, GETDATE());

INSERT INTO UserDetails (UserId, FirstName, LastName, Email, PhoneNumber, CountryCode, CreatedAt)
VALUES (1, 'Admin', 'Sistema', 'admin@bmiami.com', '1234567890', '+1', GETDATE());

-- Usuario 2: Alba Molina - Gestión de Operaciones
INSERT INTO Users (Username, PasswordHash, SuperUser, UserType, CreatedAt)
VALUES ('albalucia1907', '$2a$13$pHywiuK9AY4X/BORNdpNaeINFbvePylHLH.d6NiLEr.lUKWNEbooW', 0, 1, GETDATE());

INSERT INTO UserDetails (UserId, FirstName, LastName, Email, PhoneNumber, CountryCode, CreatedAt)
VALUES (2, 'Alba', 'Molina', 'Albalucia1907@gmail.com', '933054810', '+51', GETDATE());

-- Usuario 3: Humberto Rincon - Gestión de Operaciones
INSERT INTO Users (Username, PasswordHash, SuperUser, UserType, CreatedAt)
VALUES ('herincon2000', '$2a$13$pHywiuK9AY4X/BORNdpNaeINFbvePylHLH.d6NiLEr.lUKWNEbooW', 0, 1, GETDATE());

INSERT INTO UserDetails (UserId, FirstName, LastName, Email, PhoneNumber, CountryCode, CreatedAt)
VALUES (3, 'Humberto', 'Rincon', 'Herincon2000@gmail.com', '933054810', '+51', GETDATE());

-- Usuario 4: Jorge Javier Ardila - Manager Mantenimiento y limpieza
INSERT INTO Users (Username, PasswordHash, SuperUser, UserType, CreatedAt)
VALUES ('jjam133', '$2a$13$pHywiuK9AY4X/BORNdpNaeINFbvePylHLH.d6NiLEr.lUKWNEbooW', 0, 3, GETDATE());

INSERT INTO UserDetails (UserId, FirstName, LastName, Email, PhoneNumber, CountryCode, CreatedAt)
VALUES (4, 'Jorge Javier', 'Ardila', 'Jjam133@gmail.com', '933054810', '+51', GETDATE());

-- Usuario 5: Rafael Hurtado - Equipo de limpieza
INSERT INTO Users (Username, PasswordHash, SuperUser, UserType, CreatedAt)
VALUES ('rahurtadoc', '$2a$13$pHywiuK9AY4X/BORNdpNaeINFbvePylHLH.d6NiLEr.lUKWNEbooW', 0, 2, GETDATE());

INSERT INTO UserDetails (UserId, FirstName, LastName, Email, PhoneNumber, CountryCode, CreatedAt)
VALUES (5, 'Rafael', 'Hurtado', 'rahurtadoc@gmail.com', '933054810', '+51', GETDATE());

-- Usuario 6: Alberth Venegas - Equipo de limpieza
INSERT INTO Users (Username, PasswordHash, SuperUser, UserType, CreatedAt)
VALUES ('ajfernandez1015', '$2a$13$pHywiuK9AY4X/BORNdpNaeINFbvePylHLH.d6NiLEr.lUKWNEbooW', 0, 2, GETDATE());

INSERT INTO UserDetails (UserId, FirstName, LastName, Email, PhoneNumber, CountryCode, CreatedAt)
VALUES (6, 'Alberth', 'Venegas', 'ajfernandez1015@gmail.com', '933054810', '+51', GETDATE());

-- Usuario 7: Orlando Ramírez - Equipo de limpieza
INSERT INTO Users (Username, PasswordHash, SuperUser, UserType, CreatedAt)
VALUES ('orlandohenriqueramisa', '$2a$13$pHywiuK9AY4X/BORNdpNaeINFbvePylHLH.d6NiLEr.lUKWNEbooW', 0, 2, GETDATE());

INSERT INTO UserDetails (UserId, FirstName, LastName, Email, PhoneNumber, CountryCode, CreatedAt)
VALUES (7, 'Orlando', 'Ramírez', 'OrlandohenriqueRamisa@gmail.com', '933054810', '+51', GETDATE());

-- Usuario 8: Marggiori Colombo - Equipo de limpieza
INSERT INTO Users (Username, PasswordHash, SuperUser, UserType, CreatedAt)
VALUES ('cmarggiori', '$2a$13$pHywiuK9AY4X/BORNdpNaeINFbvePylHLH.d6NiLEr.lUKWNEbooW', 0, 2, GETDATE());

INSERT INTO UserDetails (UserId, FirstName, LastName, Email, PhoneNumber, CountryCode, CreatedAt)
VALUES (8, 'Marggiori', 'Colombo', 'Cmarggiori@gmail.com', '933054810', '+51', GETDATE());

-- Usuario 9: Andreina Santa Fe - Equipo de limpieza
INSERT INTO Users (Username, PasswordHash, SuperUser, UserType, CreatedAt)
VALUES ('andreinasantafe1', '$2a$13$pHywiuK9AY4X/BORNdpNaeINFbvePylHLH.d6NiLEr.lUKWNEbooW', 0, 2, GETDATE());

INSERT INTO UserDetails (UserId, FirstName, LastName, Email, PhoneNumber, CountryCode, CreatedAt)
VALUES (9, 'Andreina', 'Santa Fe', 'Andreinasantafe1@hotmail.com', '933054810', '+51', GETDATE());

-- Usuario 10: Carlos Toro - Equipo de limpieza
INSERT INTO Users (Username, PasswordHash, SuperUser, UserType, CreatedAt)
VALUES ('cafertoro77', '$2a$13$pHywiuK9AY4X/BORNdpNaeINFbvePylHLH.d6NiLEr.lUKWNEbooW', 0, 2, GETDATE());

INSERT INTO UserDetails (UserId, FirstName, LastName, Email, PhoneNumber, CountryCode, CreatedAt)
VALUES (10, 'Carlos', 'Toro', 'Cafertoro77@gmail.com', '933054810', '+51', GETDATE());


-- ALMACENES
-- Propiedad 137963 - Casa Tulum Great Pool Just Fun and Relax 01
INSERT INTO Stores (Name, PropertyId, CreatedAt)
VALUES ('Almacén Casa Tulum Great Pool Just Fun and Relax', 137963, GETDATE());

-- Propiedad 149538 - Casa Ibiza - Pool Spa - BBQ - Great Price 02
INSERT INTO Stores (Name, PropertyId, CreatedAt)
VALUES ('Almacén Casa Ibiza - Pool Spa - BBQ - Great Price', 149538, GETDATE());

-- Propiedad 149539 - Casa Santorini - Big Pool - BBQ - Fun & Relax 03
INSERT INTO Stores (Name, PropertyId, CreatedAt)
VALUES ('Almacén Casa Santorini - Big Pool - BBQ - Fun & Relax', 149539, GETDATE());

-- Propiedad 149540 - Mediterranean Village - Big Pool - Hot Tub -  BBQ 04
INSERT INTO Stores (Name, PropertyId, CreatedAt)
VALUES ('Almacén Mediterranean Village - Big Pool - Hot Tub -  BBQ', 149540, GETDATE());

-- Propiedad 158871 - Canalfront Pool • Family Escape • Near MIA 05
INSERT INTO Stores (Name, PropertyId, CreatedAt)
VALUES ('Almacén Canalfront Pool • Family Escape • Near MIA', 158871, GETDATE());

-- Propiedad 180153 - Arabic Villa • Heated Pool • Near Gables 07
INSERT INTO Stores (Name, PropertyId, CreatedAt)
VALUES ('Almacén Arabic Villa • Heated Pool • Near Gables', 180153, GETDATE());

-- Propiedad 191565 - Beach-Style Pool • Colorful Retreat • Near FIU 07
INSERT INTO Stores (Name, PropertyId, CreatedAt)
VALUES ('Almacén Beach-Style Pool • Colorful Retreat • Near FIU', 191565, GETDATE());

-- Propiedad 196004 - Bali Chic • Private Pool • Near MIA 08
INSERT INTO Stores (Name, PropertyId, CreatedAt)
VALUES ('Almacén Bali Chic • Private Pool • Near MIA', 196004, GETDATE());

-- Propiedad 198204 - Black house Just Perfect - Pool- BBQ - Fit12 09
INSERT INTO Stores (Name, PropertyId, CreatedAt)
VALUES ('Almacén Black house Just Perfect - Pool- BBQ - Fit12', 198204, GETDATE());

-- Propiedad 198505 - Casa Bora Bora - Just Fun Big Pool 3 Bath Fit 12 10
INSERT INTO Stores (Name, PropertyId, CreatedAt)
VALUES ('Almacén Casa Bora Bora - Just Fun Big Pool 3 Bath Fit 12', 198505, GETDATE());

-- Propiedad 213815 - Heated Pool • Family-Friendly • Near Miami Hotspot 11 | 8 excel
INSERT INTO Stores (Name, PropertyId, CreatedAt)
VALUES ('Almacén Heated Pool • Family-Friendly • Near Miami Hotspot', 213815, GETDATE());

-- Propiedad 214906 - Private Pool + BBQ • Family Oasis • Near MIA 12 | 9
INSERT INTO Stores (Name, PropertyId, CreatedAt)
VALUES ('Almacén Private Pool + BBQ • Family Oasis • Near MIA', 214906, GETDATE());

-- Propiedad 237844 - Casa Praga - Big pool & BBQ - Miami Fun - 5BR/4BA 13
INSERT INTO Stores (Name, PropertyId, CreatedAt)
VALUES ('Almacén Casa Praga - Big pool & BBQ - Miami Fun - 5BR/4BA', 237844, GETDATE());

-- Propiedad 289591 - Casa Maldivas - Blue Oasis - Heated Pool 5B/3BTH 14 | 12
INSERT INTO Stores (Name, PropertyId, CreatedAt)
VALUES ('Almacén Casa Maldivas - Blue Oasis - Heated Pool 5B/3BTH', 289591, GETDATE());

-- Propiedad 313489 - Japandi Oasis • Heated Pool • West Miami 15
INSERT INTO Stores (Name, PropertyId, CreatedAt)
VALUES ('Almacén Japandi Oasis • Heated Pool • West Miami', 313489, GETDATE());

-- Propiedad 360555 - Heated Pool • Tropical Retreat • Near MIA 17
INSERT INTO Stores (Name, PropertyId, CreatedAt)
VALUES ('Almacén Heated Pool • Tropical Retreat • Near MIA', 360555, GETDATE());

-- Propiedad 380049 - Tropical Boho • Heated Pool • Near MIA 18
INSERT INTO Stores (Name, PropertyId, CreatedAt)
VALUES ('Almacén Tropical Boho • Heated Pool • Near MIA', 380049, GETDATE());

-- Propiedad 338654 - Chicago-Style • Heated Pool + Hockey • Near FIU 16
INSERT INTO Stores (Name, PropertyId, CreatedAt)
VALUES ('Almacén Chicago-Style • Heated Pool + Hockey • Near FIU', 338654, GETDATE());

-- Propiedad 419593 - Heated Pool + Games • Family Haven • Near FIU 19
INSERT INTO Stores (Name, PropertyId, CreatedAt)
VALUES ('Almacén Heated Pool + Games • Family Haven • Near FIU', 419593, GETDATE());

-- Propiedad 435818 - Heated Pool•Family&Friends•5 min FIU•Spanish-Style 20
INSERT INTO Stores (Name, PropertyId, CreatedAt)
VALUES ('Almacén Heated Pool•Family&Friends•5 min FIU•Spanish-Style', 435818, GETDATE());

-- Propiedad 453595 - Casa prueba  21
INSERT INTO Stores (Name, PropertyId, CreatedAt)
VALUES ('Almacén prueba', 453595, GETDATE());



INSERT INTO Brands (Name, CreatedAt) VALUES ('Generico', GETDATE());
INSERT INTO Brands (Name, CreatedAt) VALUES ('Charmin', GETDATE());
INSERT INTO Brands (Name, CreatedAt) VALUES ('Bounty', GETDATE());
INSERT INTO Brands (Name, CreatedAt) VALUES ('Tide', GETDATE());
INSERT INTO Brands (Name, CreatedAt) VALUES ('Downy', GETDATE());
INSERT INTO Brands (Name, CreatedAt) VALUES ('Clorox', GETDATE());
INSERT INTO Brands (Name, CreatedAt) VALUES ('Windex', GETDATE());
INSERT INTO Brands (Name, CreatedAt) VALUES ('Fabuloso', GETDATE());
INSERT INTO Brands (Name, CreatedAt) VALUES ('Cascade', GETDATE());
INSERT INTO Brands (Name, CreatedAt) VALUES ('Dove', GETDATE());
INSERT INTO Brands (Name, CreatedAt) VALUES ('Pantene', GETDATE());
INSERT INTO Brands (Name, CreatedAt) VALUES ('Lysol', GETDATE());
INSERT INTO Brands (Name, CreatedAt) VALUES ('Folgers', GETDATE());
INSERT INTO Brands (Name, CreatedAt) VALUES ('Reynolds', GETDATE());
INSERT INTO Brands (Name, CreatedAt) VALUES ('Dawn', GETDATE());
INSERT INTO Brands (Name, CreatedAt) VALUES ('Air Wick', GETDATE());
INSERT INTO Brands (Name, CreatedAt) VALUES ('Febreze', GETDATE());
INSERT INTO Brands (Name, CreatedAt) VALUES ('Mr. Clean', GETDATE());

INSERT INTO Categories (Name, ParentId, CreatedAt) VALUES ('Higiene Personal', NULL, GETDATE()); -- 1
INSERT INTO Categories (Name, ParentId, CreatedAt) VALUES ('Limpieza General', NULL, GETDATE()); -- 2
INSERT INTO Categories (Name, ParentId, CreatedAt) VALUES ('Lavandería', NULL, GETDATE()); -- 3
INSERT INTO Categories (Name, ParentId, CreatedAt) VALUES ('Cocina', NULL, GETDATE()); -- 4
INSERT INTO Categories (Name, ParentId, CreatedAt) VALUES ('Baño', NULL, GETDATE()); -- 5
INSERT INTO Categories (Name, ParentId, CreatedAt) VALUES ('Consumibles', NULL, GETDATE()); -- 6
INSERT INTO Categories (Name, ParentId, CreatedAt) VALUES ('Desechables', NULL, GETDATE()); -- 7
INSERT INTO Categories (Name, ParentId, CreatedAt) VALUES ('Aromatizantes', NULL, GETDATE()); -- 8
INSERT INTO Categories (Name, ParentId, CreatedAt) VALUES ('Muebles y Electrodomésticos', NULL, GETDATE()); -- 9

-- -- Guest 1
-- INSERT INTO Guests (FirstName, LastName, Email, PhoneNumber, CreatedAt)
-- VALUES ('Michael', 'Johnson', 'michael.johnson@email.com', '+1-305-555-1001', GETDATE());

-- -- Guest 2
-- INSERT INTO Guests (FirstName, LastName, Email, PhoneNumber, CreatedAt)
-- VALUES ('Sarah', 'Williams', 'sarah.williams@email.com', '+1-305-555-1002', GETDATE());

-- -- Guest 3
-- INSERT INTO Guests (FirstName, LastName, Email, PhoneNumber, CreatedAt)
-- VALUES ('David', 'Brown', 'david.brown@email.com', '+1-305-555-1003', GETDATE());

-- -- Guest 4
-- INSERT INTO Guests (FirstName, LastName, Email, PhoneNumber, CreatedAt)
-- VALUES ('Jennifer', 'Davis', 'jennifer.davis@email.com', '+1-305-555-1004', GETDATE());

-- -- Guest 5
-- INSERT INTO Guests (FirstName, LastName, Email, PhoneNumber, CreatedAt)
-- VALUES ('Robert', 'Miller', 'robert.miller@email.com', '+1-305-555-1005', GETDATE());

-- -- Guest 6
-- INSERT INTO Guests (FirstName, LastName, Email, PhoneNumber, CreatedAt)
-- VALUES ('Emily', 'Wilson', 'emily.wilson@email.com', '+1-305-555-1006', GETDATE());

-- -- Guest 7
-- INSERT INTO Guests (FirstName, LastName, Email, PhoneNumber, CreatedAt)
-- VALUES ('James', 'Moore', 'james.moore@email.com', '+1-305-555-1007', GETDATE());

-- -- Guest 8
-- INSERT INTO Guests (FirstName, LastName, Email, PhoneNumber, CreatedAt)
-- VALUES ('Lisa', 'Taylor', 'lisa.taylor@email.com', '+1-305-555-1008', GETDATE());

-- -- Guest 9
-- INSERT INTO Guests (FirstName, LastName, Email, PhoneNumber, CreatedAt)
-- VALUES ('Christopher', 'Anderson', 'christopher.anderson@email.com', '+1-305-555-1009', GETDATE());

-- -- Guest 10
-- INSERT INTO Guests (FirstName, LastName, Email, PhoneNumber, CreatedAt)
-- VALUES ('Amanda', 'Thomas', 'amanda.thomas@email.com', '+1-305-555-1010', GETDATE());

-- GO



