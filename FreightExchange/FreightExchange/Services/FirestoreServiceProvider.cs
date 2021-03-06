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
        public static async Task<bool> InsertContractInFirestore(Models.ContractModel contract)
        {
            await CrossCloudFirestore
                .Current
                .Instance
                .Collection(Models.ContractModel.CollectionPath)
                .Document().SetAsync(contract);
            return true;
        }
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

        public static async Task<bool> UpdateCarrierOffertAsync(Models.CarrierModel offert)
        {
            await CrossCloudFirestore
                .Current
                .Instance
                .Collection(Models.CarrierModel.CollectionPath)
                .Document(offert.Id)
                .UpdateAsync(offert);
            return true;
        }

        public static async Task<List<Models.CarrierModel>> GetFirestoreAllCarrierOffertsForUserAsync(string user_id)
        {
            IQuerySnapshot query = await CrossCloudFirestore.Current
                                     .Instance
                                     .Collection(Models.CarrierModel.CollectionPath)
                                     .WhereEqualsTo("carrier_id", user_id)
                                     .GetAsync();

            IEnumerable<Models.CarrierModel> orders = query.ToObjects<Models.CarrierModel>();
            return new List<Models.CarrierModel>(orders);
        }

        public static async Task<List<Models.CarrierModel>> GetFirestoreAllCarrierOffertsAsync()
        {
            IQuerySnapshot query = await CrossCloudFirestore.Current
                                     .Instance
                                     .Collection(Models.CarrierModel.CollectionPath)
                                     .GetAsync();

            IEnumerable<Models.CarrierModel> orders = query.ToObjects<Models.CarrierModel>();
            return new List<Models.CarrierModel>(orders);
        }

        public static async Task<List<Models.CarrierModel>> GetFirestoreAllCarrierOffertsByStartPlaceAsync(string startPlace)
        {
            IQuerySnapshot query = await CrossCloudFirestore.Current
                                     .Instance
                                     .Collection(Models.CarrierModel.CollectionPath)
                                     .WhereEqualsTo("StartPlace", startPlace)
                                     .GetAsync();

            IEnumerable<Models.CarrierModel> orders = query.ToObjects<Models.CarrierModel>();
            return new List<Models.CarrierModel>(orders);
        }

        public static async Task<bool> DeleteCarrierOffAsync(Models.CarrierModel order)
        {
            await _cloud.Collection(Models.CarrierModel.CollectionPath)
                        .Document(order.Id)
                        .DeleteAsync();

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

        public static async Task<bool> UpdateOrderAsync(Models.OrderModel order)
        {
            
            order.Id = order.Id == null ? order.Id_value : order.Id;
            await CrossCloudFirestore
                .Current
                .Instance
                .Collection(Models.OrderModel.CollectionPath)
                .Document(order.Id )
                .UpdateAsync(order);
            return true;
        }

        public static async Task<List<Models.OrderModel>> GetFirestoreAllOrdersForUserAsync(string user_id)
        {
            IQuerySnapshot query = await CrossCloudFirestore.Current
                                     .Instance
                                     .Collection(Models.OrderModel.CollectionPath)
                                     .WhereEqualsTo("client_id", user_id)
                                     .GetAsync();

            IEnumerable<Models.OrderModel> orders = query.ToObjects<Models.OrderModel>();
            return new List<Models.OrderModel>(orders);
        }

        public static async Task<bool> DeleteOrderAsync(Models.OrderModel order)
        {
            await _cloud.Collection(Models.OrderModel.CollectionPath)
                        .Document(order.Id_value)
                        .DeleteAsync();

            return true;
        }

        public static async Task<List<Models.OrderModel>> GetFirestoreAllOrdersForCarrierAsync(List<string> carrier_id)
        {
            IQuerySnapshot query = await CrossCloudFirestore.Current
                                     .Instance
                                     .Collection(Models.OrderModel.CollectionPath)
                                     .WhereIn("id", carrier_id)
                                     .GetAsync();

            IEnumerable<Models.OrderModel> orders = query.ToObjects<Models.OrderModel>();
            return new List<Models.OrderModel>(orders);
        }
        #endregion

        #region Contract
        public static async Task<List<Models.ContractModel>> GetFirestoreContractsLessThen()
        {
            IQuerySnapshot query = await CrossCloudFirestore.Current
                                     .Instance
                                     .Collection(Models.ContractModel.CollectionPath)
                                     .GetAsync();

            IEnumerable<Models.ContractModel> users = query.ToObjects<Models.ContractModel>();
            return new List<Models.ContractModel>(users);
        }

        public static async Task<List<Models.ContractModel>> GetFirestoreContractsForAClient(string id_client, string label)
        {
            IQuerySnapshot query = await CrossCloudFirestore.Current
                                     .Instance
                                     .Collection(Models.ContractModel.CollectionPath)
                                     .WhereEqualsTo(label, id_client)
                                     .GetAsync();

            IEnumerable<Models.ContractModel> users = query.ToObjects<Models.ContractModel>();
            return new List<Models.ContractModel>(users);
        }
        #endregion  
    }
}
