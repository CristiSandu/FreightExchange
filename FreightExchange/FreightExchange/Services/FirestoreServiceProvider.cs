using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FreightExchange.Services
{
    public static class FirestoreServiceProvider
    {
        private static IFirestore _cloud = CrossCloudFirestore.Current.Instance;
        #region User
        public static async Task<bool> CreateUserFirestore(Models.UserModel user)
        {
            await CrossCloudFirestore
                .Current
                .Instance
                .Collection(Models.UserModel.CollectionPath)
                .Document(user.Id).SetAsync(user);
            return true;
        }

        public static async Task<Models.UserModel> GetFirestoreUser(string id)
        {
            IDocumentSnapshot document = await CrossCloudFirestore.Current
                                        .Instance
                                        .Collection(Models.UserModel.CollectionPath)
                                        .Document(id)
                                        .GetAsync();
            Models.UserModel user = document.ToObject<Models.UserModel>();
            return user;
        }

        public static async Task<bool> DeleteUser(Models.UserModel user)
        {
            await _cloud.Collection(Models.UserModel.CollectionPath)
                        .Document(user.Id)
                        .DeleteAsync();

            return true;
        }

        public static async Task<List<Models.UserModel>> GetFirestoreAllUser(string role)
        {
            IQuerySnapshot query = await CrossCloudFirestore.Current
                                     .Instance
                                     .Collection(Models.UserModel.CollectionPath)
                                     .WhereEqualsTo("rol", role)
                                     .GetAsync();

            IEnumerable<Models.UserModel> users = query.ToObjects<Models.UserModel>();
            return new List<Models.UserModel>(users);
        }
        #endregion

        #region Goods
        public static async Task<bool> CreateGoodAsync(Models.GoodsModel good)
        {
            await CrossCloudFirestore
                .Current
                .Instance
                .Collection(Models.GoodsModel.CollectionPath)
                .Document(good.Name).SetAsync(good);
            return true;
        }
        public static async Task<bool> DeleteGoodAsync(Models.GoodsModel good)
        {
            await _cloud.Collection(Models.GoodsModel.CollectionPath)
                        .Document(good.Name)
                        .DeleteAsync();

            return true;
        }

        public static async Task<bool> ExistGood(string id)
        {
            IDocumentSnapshot document = await CrossCloudFirestore.Current
                                        .Instance
                                        .Collection(Models.GoodsModel.CollectionPath)
                                        .Document(id)
                                        .GetAsync();
            Models.GoodsModel user = document.ToObject<Models.GoodsModel>();
            return user != null;
        }

        public static async Task<List<Models.GoodsModel>> GetFirestoreAllGoodsAsync()
        {
            IQuerySnapshot query = await CrossCloudFirestore.Current
                                     .Instance
                                     .Collection(Models.GoodsModel.CollectionPath)
                                     .OrderBy("type")
                                     .GetAsync();

            IEnumerable<Models.GoodsModel> users = query.ToObjects<Models.GoodsModel>();
            return new List<Models.GoodsModel>(users);
        }
        #endregion

        #region Carrier
        public static async Task<bool> CreateCarrierOffertAsync(Models.CarrierModel offert)
        {
            await CrossCloudFirestore
                .Current
                .Instance
                .Collection(Models.CarrierModel.CollectionPath)
                .Document().SetAsync(offert);
            return true;
        }
        #endregion

        #region Order
        public static async Task<bool> CreateOrderAsync(Models.OrderModel order)
        {
            await CrossCloudFirestore
                .Current
                .Instance
                .Collection(Models.OrderModel.CollectionPath)
                .Document().SetAsync(order);
            return true;
        }
        #endregion
    }
}
