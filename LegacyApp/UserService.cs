using System;

namespace LegacyApp
{
    public class UserService
    {
        private readonly ClientRepository clientRepository;
        private readonly UserCreditServiceClient userCreditService;

        public UserService()
        {
            clientRepository = new ClientRepository();
            userCreditService = new UserCreditServiceClient();
        }

        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfUserBirth, int clientId)
        {
            if (IsNotValidFirstNameOrLastName(firstName, lastName))
                return false;

            if (!IsValidEmail(email))
                return false;

            if (CalculateUserAge(dateOfUserBirth) < 21)
                return false;

            var client = clientRepository.GetById(clientId);

            var user = new User
            {
                Client = client,
                DateOfBirth = dateOfUserBirth,
                EmailAddress = email,
                Firstname = firstName,
                Surname = lastName
            };

            if (client.Name == "VeryImportantClient")
                user.HasCreditLimit = false;
            else
            {
                user.HasCreditLimit = true;
                var creditLimit = userCreditService.GetCreditLimit(firstName, lastName, dateOfUserBirth);

                if (client.Name == "ImportantClient")
                    creditLimit *= 2;

                user.CreditLimit = creditLimit;
            }

            if (user.HasCreditLimit && user.CreditLimit < 500)
                return false;

            UserDataAccess.AddUser(user);

            return true;
        }

        private bool IsNotValidFirstNameOrLastName(string firstName, string lastName)
            => string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName);

        private bool IsValidEmail(string email)
            => email.Contains('@') && email.Contains('.');

        private int CalculateUserAge(DateTime dateOfUserBirth)
        {
            var todayDate = DateTime.Now;
            int userFullAge = todayDate.Year - dateOfUserBirth.Year;

            if (todayDate.Month < dateOfUserBirth.Month
                || (todayDate.Month == dateOfUserBirth.Month && todayDate.Day < dateOfUserBirth.Day))
            {
                userFullAge -= 1;
            }

            return userFullAge;
        }
    }
}