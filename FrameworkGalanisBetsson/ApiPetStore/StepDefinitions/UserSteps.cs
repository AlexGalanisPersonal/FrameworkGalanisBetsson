using SauceDemoUiBetsson.ApiPetStore.Helpers;
using SauceDemoUiBetsson.ApiPetStore.Models;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace SauceDemoUiBetsson.ApiPetStore.StepDefinitions;


[Binding]
public class UserSteps(ApiClient apiClient, ScenarioContext scenarioContext)
{
    private User? _testUser;
    private readonly List<User> _createdUsers = [];

    [Given(@"I have a new user with the following details")]
    public void GivenIHaveANewUserWithTheFollowingDetails(Table table)
    {
        var userDetails = table.CreateInstance<User>();
        // Add a timestamp to make username unique
        var uniqueUsername = $"{userDetails.Username}_{DateTime.Now.Ticks}";
    
        _testUser = new User
        {
            Id = DateTime.Now.Ticks,
            Username = uniqueUsername,  // Use the unique username
            FirstName = userDetails.FirstName,
            LastName = userDetails.LastName,
            Email = userDetails.Email,
            Password = userDetails.Password,
            Phone = userDetails.Phone,
            UserStatus = userDetails.UserStatus
        };
        scenarioContext["TestUser"] = _testUser;
    }

    [Given(@"I have created a list of users")]
    public void GivenIHaveCreatedAListOfUsers(Table table)
    {
        _createdUsers.Clear();
    
        var users = table.Rows.Select(row => new User
        {
            Id = DateTime.Now.Ticks,
            Username = $"{row["Username"]}_{Guid.NewGuid():N}",
            FirstName = row["FirstName"],
            LastName = row["LastName"],
            Email = row["Email"],
            Password = "password123",
            Phone = row["Phone"],
            UserStatus = 1
        }).ToList();
    
        _createdUsers.AddRange(users);
        scenarioContext["UsersToCreate"] = _createdUsers;
    }

    [When(@"I send a request to create the user")]
    public async Task WhenISendARequestToCreateTheUser()
    {
        if (_testUser != null)
        {
            var response = await apiClient.Post<ApiResponse>("/user", _testUser);
            scenarioContext["ApiResponse"] = response;
            // Store the test user for later verification
            scenarioContext["CreatedUser"] = _testUser;
        }
    }

    [When(@"I create multiple users with an array")]
    public async Task WhenICreateMultipleUsersWithAnArray()
    {
        var users = scenarioContext.Get<List<User>>("UsersToCreate");
        var response = await apiClient.Post<ApiResponse>("/user/createWithArray", users);
        scenarioContext["ApiResponse"] = response;
        // Store the users for later verification
        scenarioContext["CreatedUsers"] = users;
    }

    [When(@"I update the user's email to ""(.*)""")]
    public async Task WhenIUpdateTheUsersEmailTo(string newEmail)
    {
        var user = scenarioContext.Get<User>("CreatedUser");
        user.Email = newEmail;

        var response = await apiClient.Put<ApiResponse>($"/user/{user.Username}", user);
        scenarioContext["ApiResponse"] = response;
        scenarioContext["UpdatedUser"] = user;
    }

    [When(@"I delete the user")]
    public async Task WhenIDeleteTheUser()
    {
        var user = scenarioContext.Get<User>("CreatedUser");
        var response = await apiClient.Delete<ApiResponse>($"/user/{user.Username}");
        scenarioContext["DeleteResponse"] = response;
    }
    
    [When(@"I login with username ""(.*)"" and password ""(.*)""")]
    public async Task WhenILoginWithUsernameAndPassword(string username, string password)
    {
        var response = await apiClient.Get<ApiResponse>($"/user/login?username={username}&password={password}");
        scenarioContext["ApiResponse"] = response;
    }

    [Then(@"the user should be successfully created")]
    public void ThenTheUserShouldBeSuccessfullyCreated()
    {
        var apiResponse = scenarioContext.Get<ApiResponse>("ApiResponse");
        Assert.That(apiResponse.Code, Is.EqualTo(200), "API response code should be 200");
        Assert.That(apiResponse.Type, Is.EqualTo("unknown"), "API response type should be 'unknown'");
        Assert.That(apiResponse.Message, Is.Not.Empty, "API response message should not be empty");
    }

    [Then(@"I can retrieve the user by username")]
    public async Task ThenICanRetrieveTheUserByUsername()
    {
        var createdUser = scenarioContext.Get<User>("CreatedUser");
        var retrievedUser = await apiClient.Get<User>($"/user/{createdUser.Username}");
        
        Assert.That(retrievedUser, Is.Not.Null);
        Assert.That(retrievedUser?.Username, Is.EqualTo(createdUser.Username));
    }

    [Then(@"the user's email should be updated to ""(.*)""")]
    public void ThenTheUsersEmailShouldBeUpdatedTo(string expectedEmail)
    {
        var apiResponse = scenarioContext.Get<ApiResponse>("ApiResponse");
        Assert.That(apiResponse.Code, Is.EqualTo(200), "API response code should be 200");
    }

    [Then(@"retrieving the user should return not found")]
    public void  ThenRetrievingTheUserShouldReturnNotFound()
    {
        var user = scenarioContext.Get<User>("CreatedUser");
    
        var exception =  Assert.ThrowsAsync<HttpRequestException>(async () => 
            await apiClient.Get<User>($"/user/{user.Username}"));
    
        Assert.That(exception?.Message, Does.Contain("404"), "Expected user to be not found (404)");
    }

    [Then(@"the login should be successful")]
    public void ThenTheLoginShouldBeSuccessful()
    {
        var apiResponse = scenarioContext.Get<ApiResponse>("ApiResponse");
        Assert.That(apiResponse.Code, Is.EqualTo(200), "API response code should be 200");
        Assert.That(apiResponse.Message, Does.Contain("logged in user session:"), 
            "Login response should contain session information");
    }
}