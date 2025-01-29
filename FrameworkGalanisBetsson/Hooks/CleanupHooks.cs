using SauceDemoUiBetsson.ApiPetStore.Helpers;
using SauceDemoUiBetsson.ApiPetStore.Models;
using TechTalk.SpecFlow;

namespace SauceDemoUiBetsson.Hooks;

[Binding]
public class CleanupHooks
{
    private readonly ScenarioContext _scenarioContext;
    private readonly ApiClient _apiClient;

    public CleanupHooks(ScenarioContext scenarioContext, ApiClient apiClient)
    {
        _scenarioContext = scenarioContext;
        _apiClient = apiClient;
    }

    [AfterScenario("api")]
    public async Task CleanupTestData()
    {
        var scenarioTags = _scenarioContext.ScenarioInfo.Tags;
        
        // Cleanup created pets if not in a delete pet scenario
        if (!scenarioTags.Contains("DeletePet") && _scenarioContext.ContainsKey("CreatedPets"))
        {
            var pets = _scenarioContext.Get<List<Pet>>("CreatedPets");
            foreach (var pet in pets)
            {
                await _apiClient.Delete<ApiResponse>($"/pet/{pet.Id}");
            }
        }

        // Cleanup single test pet if not in a delete pet scenario
        if (!scenarioTags.Contains("DeletePet") && _scenarioContext.ContainsKey("TestPet"))
        {
            var pet = _scenarioContext.Get<Pet>("TestPet");
            await _apiClient.Delete<ApiResponse>($"/pet/{pet.Id}");
        }

        // Cleanup created users if not in a delete user scenario
        if (!scenarioTags.Contains("DeleteUser") && _scenarioContext.ContainsKey("CreatedUsers"))
        {
            var users = _scenarioContext.Get<List<User>>("CreatedUsers");
            foreach (var user in users)
            {
                await _apiClient.Delete<ApiResponse>($"/user/{user.Username}");
            }
        }

        // Cleanup single test user if not in a delete user scenario
        if (!scenarioTags.Contains("DeleteUser") && _scenarioContext.ContainsKey("CreatedUser"))
        {
            var user = _scenarioContext.Get<User>("CreatedUser");
            await _apiClient.Delete<ApiResponse>($"/user/{user.Username}");
        }

        // Cleanup test orders if not in a delete order scenario
        if (!scenarioTags.Contains("DeleteOrder") && _scenarioContext.ContainsKey("TestOrder"))
        {
            var order = _scenarioContext.Get<StoreOrder>("TestOrder");
            await _apiClient.Delete<ApiResponse>($"/store/order/{order.Id}");
        }
    }
}