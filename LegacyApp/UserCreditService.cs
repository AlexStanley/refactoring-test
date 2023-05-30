using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using GrpcService.Protos;
using System;

namespace LegacyApp
{
    public partial class UserCreditServiceClient
    {
        private readonly string _url = "https://totally-real-service.com/IUserCreditService/GetCreditLimit";
        public int GetCreditLimit(string firstNane, string lastName, DateTime dateOfBirth)
        {
            var channel = GrpcChannel.ForAddress(_url);
            var userClient = new UserCredit.UserCreditClient(channel);

            UserCreditRequest request = new UserCreditRequest
            {
                Firstname = firstNane,
                Surname = lastName,
                DateOfBirth = dateOfBirth.ToTimestamp()
            };

            var creditLimit = userClient.GetCreditLimit(request);

            return creditLimit.CreditLimit;
        }
    }
}