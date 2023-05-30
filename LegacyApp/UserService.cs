using System;

namespace LegacyApp
{
    public class UserService
    {
        private readonly string _veryImportantClient = "VeryImportantClient";
        private readonly string _importantClient = "ImportantClient";
        private readonly char _at = '@';
        private readonly char _dot = '.';

        public ClientRepository clientRepository { get; set; }
        public UserCreditServiceClient userCreditService { get; set; }

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

            if (client.Name == _veryImportantClient)
                user.HasCreditLimit = false;
            else
            {
                user.HasCreditLimit = true;
                var creditLimit = userCreditService.GetCreditLimit(firstName, lastName, dateOfUserBirth);

                if (client.Name == _importantClient)
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
            => email.Contains(_at) && email.Contains(_dot);

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