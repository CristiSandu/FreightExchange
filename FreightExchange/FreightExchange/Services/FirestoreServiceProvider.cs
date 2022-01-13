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
    }
}
