using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FreightExchange.Services
{
    public static class FirestoreServiceProvider
    {
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
    }
}
