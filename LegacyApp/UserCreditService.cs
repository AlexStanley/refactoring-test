using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using GrpcService.Protos;
using System;

namespace LegacyApp
{
    public partial class UserCreditServiceClient
    {
       public int GetCreditLimit(string firstNane, string lastName, DateTime dateOfBirth)
        {
            var channel = GrpcChannel.ForAddress("http://totally-real-service.com/IUserCreditService/GetCreditLimit");
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