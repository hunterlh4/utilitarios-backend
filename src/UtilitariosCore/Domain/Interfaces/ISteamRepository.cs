using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Domain.Interfaces;

public interface ISteamRepository
{
    Task<IEnumerable<SteamItem>> GetAllItems();
    Task<SteamItem> GetByIdItems(int id);
    Task<int> CreateItems(SteamItem item);
    Task<bool> UpdateItems(SteamItem item);
    Task<bool> DeleteItems(int id);
    Task<bool> ExistsItems(int id);
    Task<bool> ExistsByExternalIdItems(string externalId);

    #region steam-item-drop
    Task<IEnumerable<SteamItemDrop>> GetAllDrops();
    Task<SteamItemDrop> GetByIdDrops(int id);
    Task<int> CreateDrops(SteamItemDrop drop);
    Task<bool> UpdateDrops(SteamItemDrop drop);
    Task<bool> DeleteDrops(int id);
    Task<bool> ExistsDrops(int id);
    #endregion

    #region steam-item-purchase
    Task<IEnumerable<SteamItemPurchase>> GetAllPurchase();
    Task<SteamItemPurchase> GetByIdPurchase(int id);
    Task<int> CreatePurchase(SteamItemPurchase purchase);
    Task<bool> UpdatePurchase(SteamItemPurchase purchase);
    Task<bool> DeletePurchase(int id);
    Task<bool> ExistsPurchase(int id);
    #endregion
    
    #region steam-item-cache
    Task<IEnumerable<DotaCache>> GetAllCache();
    Task<IEnumerable<DotaCache>> GetByTreasureIdCache(int treasureId);
    Task<DotaCache?> GetByIdCache(int id);
    Task<int> CreateCache(DotaCache cache);
    Task<bool> UpdateCache(DotaCache cache);
    Task<bool> DeleteCache(int id);
    Task<bool> ExistsCache(int id);
    #endregion

    #region steam-hero
    Task<IEnumerable<DotaHero>> GetAllHero();
    Task<DotaHero?> GetByIdHero(int id);
    Task<int> CreateHero(DotaHero hero);
    Task<bool> UpdateHero(DotaHero hero);
    Task<bool> DeleteHero(int id);
    Task<bool> ExistsHero(int id);
    #endregion

    #region steam-treasure
    Task<IEnumerable<DotaTreasure>> GetAllTreasure();
    Task<DotaTreasure?> GetByIdTreasure(int id);
    Task<int> CreateTreasure(DotaTreasure treasure);
    Task<bool> UpdateTreasure(DotaTreasure treasure);
    Task<bool> DeleteTreasure(int id);
    Task<bool> ExistsTreasure(int id);
    #endregion

}
