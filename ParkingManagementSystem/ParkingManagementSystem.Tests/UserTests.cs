using ParkingManagementSystem.DAL.Data;
using ParkingManagementSystem.DAL.Models;
using ParkingManagementSystem.DAL.Repositories;
using Microsoft.Data.SqlClient;
namespace ParkingManagementSystem.Tests
{
    [TestFixture]
    public class UserTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test, Order(1)]
        public void DatabaseConnectionOpensSuccessfuly()
        {
            DatabaseConnector databaseConnector = new DatabaseConnector();

            SqlConnection connection = databaseConnector.GetOpenConnection();

            Assert.IsNotNull(connection);
        }

        [Test, Order(2)]
        public async Task UserCreatesAccountSuccessfuly()
        {

            DatabaseConnector databaseConnector = new DatabaseConnector();
            UserRepository userRepository = new UserRepository(databaseConnector);

            User user = new User();
            user.FirstName = "Test";
            user.LastName = "Test";
            user.Email = "test1@email.com";
            user.PasswordHash = "Test123test";
            user.Phone = "123456789";

            await userRepository.CreateUser(user);

            User userFromDatabase = await userRepository.GetUserByEmail(user.Email);

            Assert.IsNotNull(userFromDatabase);
            await userRepository.DeleteUser(userFromDatabase.UserID, user.PasswordHash);
        }

        [Test, Order(3)]
        public async Task GetsUserByIdSuccessfuly()
        {
            DatabaseConnector databaseConnector = new DatabaseConnector();
            UserRepository userRepository = new UserRepository(databaseConnector);

            User user = new User();
            user.FirstName = "Test";
            user.LastName = "Test";
            user.Email = "test2@email.com";
            user.PasswordHash = "Test123test";
            user.Phone = "123456789";

            await userRepository.CreateUser(user);

            User userFromDatabase = await userRepository.GetUserByEmail(user.Email);
            userFromDatabase = await userRepository.GetUserByIdAsync(userFromDatabase.UserID);

            Assert.IsNotNull(user);

            await userRepository.DeleteUser(userFromDatabase.UserID, user.PasswordHash);
        }

        [Test, Order(4)]
        public async Task GetsUserByEmailSuccessfuly()
        {
            DatabaseConnector databaseConnector = new DatabaseConnector();
            UserRepository userRepository = new UserRepository(databaseConnector);

            User user = new User();
            user.FirstName = "Test";
            user.LastName = "Test";
            user.Email = "test3@email.com";
            user.PasswordHash = "Test123test";
            user.Phone = "123456789";

            await userRepository.CreateUser(user);

            User userFromDatabase = await userRepository.GetUserByEmail(user.Email);

            Assert.IsNotNull(userFromDatabase);

            await userRepository.DeleteUser(userFromDatabase.UserID, user.PasswordHash);
        }

        [Test, Order(5)]
        public async Task UserCanAuthenticateSuccessfuly()
        {
            DatabaseConnector databaseConnector = new DatabaseConnector();
            UserRepository userRepository = new UserRepository(databaseConnector);

            User user = new User();
            user.FirstName = "Test";
            user.LastName = "Test";
            user.Email = "test@email.com";
            user.PasswordHash = "Test123test";
            user.Phone = "123456789";

            await userRepository.CreateUser(user);

            User userFromDatabase = await userRepository.GetUserByEmail(user.Email);

            if (await userRepository.AuthenticateUser(user.Email, user.PasswordHash))
            {
                Assert.Pass();
                await userRepository.DeleteUser(userFromDatabase.UserID, user.PasswordHash);
            }
            else
            {
                Assert.Fail();
                await userRepository.DeleteUser(userFromDatabase.UserID, user.PasswordHash);
            }
        }

        [Test, Order(6)]
        public async Task UserCanBeUpdatedSuccessfuly()
        {
            DatabaseConnector databaseConnector = new DatabaseConnector();
            UserRepository userRepository = new UserRepository(databaseConnector);

            User user = new User();
            user.FirstName = "Test";
            user.LastName = "Test";
            user.Email = "test@email.com";
            user.PasswordHash = "Test123test";
            user.Phone = "123456789";

            await userRepository.CreateUser(user);

            User userFromDatabase = await userRepository.GetUserByEmail(user.Email);

            userFromDatabase.FirstName = "Test2";

            if (await userRepository.UpdateUser(userFromDatabase, user.PasswordHash))
            {
                userFromDatabase = await userRepository.GetUserByEmail(user.Email);

                if (await userRepository.DeleteUserById(userFromDatabase.UserID))
                {
                    Assert.That(userFromDatabase.FirstName, Is.EqualTo("Test2"));
                }

            }
            else
            {
                Assert.Fail();
            }

        }

        [Test, Order(7)]
        public async Task AllUsersCanBeFetchedSuccessfuly()
        {
            DatabaseConnector databaseConnector = new DatabaseConnector();
            UserRepository userRepository = new UserRepository(databaseConnector);

            User user = new User();
            user.FirstName = "Test";
            user.LastName = "Test";
            user.Email = "test@email.com";
            user.PasswordHash = "Test123test";
            user.Phone = "123456789";

            await userRepository.CreateUser(user);

            List<User> users = await userRepository.GetAllUsers();

            Assert.That(users.Count, Is.GreaterThan(0));

            await userRepository.DeleteUser(users[0].UserID, user.PasswordHash);
        }

        [Test, Order(8)]
        public async Task UserCanBePromotedSuccessfuly()
        {
            DatabaseConnector databaseConnector = new DatabaseConnector();
            UserRepository userRepository = new UserRepository(databaseConnector);

            User user = new User();
            user.FirstName = "Test";
            user.LastName = "Test";
            user.Email = "test@email.com";
            user.PasswordHash = "Test123test";
            user.Phone = "123456789";

            await userRepository.CreateUser(user);

            User userFromDatabase = await userRepository.GetUserByEmail(user.Email);

            await userRepository.PromoteUser(userFromDatabase.UserID);

            userFromDatabase = await userRepository.GetUserByEmail(user.Email);

            Assert.That(userFromDatabase.IsAdmin, Is.EqualTo(true));

            await userRepository.DeleteUser(userFromDatabase.UserID, user.PasswordHash);
        }
    }
}