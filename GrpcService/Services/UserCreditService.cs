using Grpc.Core;
using GrpcService.Protos;

namespace GrpcService.Services
{
    public class UserCreditService : UserCredit.UserCreditBase
    {
        private readonly ILogger<UserCreditService> _logger;
        public UserCreditService(ILogger<UserCreditService> logger)
        {
            _logger = logger;
        }

        public override Task<UserCreditResponse> GetCreditLimit(UserCreditRequest request, ServerCallContext context)
        {
            return base.GetCreditLimit(request, context);
        }
    }
}
