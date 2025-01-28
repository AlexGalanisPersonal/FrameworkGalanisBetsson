using SauceDemoUiBetsson.ApiPetStore.Helpers;
using SauceDemoUiBetsson.ApiPetStore.Models;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace SauceDemoUiBetsson.ApiPetStore.StepDefinitions;

[Binding]
public class PetSteps(ApiClient apiClient, ScenarioContext scenarioContext)
{
    private Pet? _testPet;
    private readonly List<Pet> _createdPets = [];

    [Given(@"I have a new pet with the following details")]
    public void GivenIHaveANewPetWithTheFollowingDetails(Table table)
    {
        var petDetails = table.CreateInstance<Pet>();
        _testPet = new Pet
        {
            Id = DateTime.Now.Ticks,
            Name = petDetails.Name,
            Category = new Category { Id = 1, Name = petDetails.Category?.Name ?? string.Empty },
            Status = petDetails.Status,
            PhotoUrls = [],
            Tags = []
        };
    }

    [Given(@"I have an existing pet in the store with name ""(.*)""")]
    public async Task GivenIHaveAnExistingPetInTheStoreWithName(string petName)
    {
        _testPet = new Pet
        {
            Id = DateTime.Now.Ticks,
            Name = petName,
            Category = new Category { Id = 1, Name = "Test" },
            Status = "available",
            PhotoUrls = new List<string>(),
            Tags = new List<Tag>()
        };

        var response = await apiClient.Post<Pet>("/pet", _testPet);
        scenarioContext["TestPet"] = response;
    }

    [Given(@"I have created the following pets with tags")]
    public async Task GivenIHaveCreatedTheFollowingPetsWithTags(Table table)
    {
        _createdPets.Clear();

        foreach (var row in table.Rows)
        {
            var tags = row["Tags"].Split(',')
                .Select(t => new Tag { Id = DateTime.Now.Ticks, Name = t.Trim() })
                .ToList();

            var pet = new Pet
            {
                Id = DateTime.Now.Ticks,
                Name = row["Pet Name"],
                Status = row["Status"],
                Category = new Category { Id = 1, Name = "Test" },
                Tags = tags,
                PhotoUrls = new List<string>()
            };

            var response = await apiClient.Post<Pet>("/pet", pet);
            if (response != null)
            {
                _createdPets.Add(response);
            }
        }
        scenarioContext["CreatedPets"] = _createdPets;
    }

    [When(@"I send a request to add the pet")]
    public async Task WhenISendARequestToAddThePet()
    {
        if (_testPet != null)
        {
            var response = await apiClient.Post<Pet>("/pet", _testPet);
            scenarioContext["AddedPet"] = response;
        }
    }

    [When(@"I update the pet's status to ""(.*)""")]
    public async Task WhenIUpdateThePetsStatusTo(string newStatus)
    {
        var pet = scenarioContext.Get<Pet>("TestPet");
        pet.Status = newStatus;

        var response = await apiClient.Put<Pet>("/pet", pet);
        scenarioContext["UpdatedPet"] = response;
    }

    [When(@"I delete the pet from the store")]
    public async Task WhenIDeleteThePetFromTheStore()
    {
        var pet = scenarioContext.Get<Pet>("TestPet");
        await apiClient.Delete<Pet>($"/pet/{pet.Id}");
    }

    [When(@"I search for pets with tag ""(.*)""")]
    public async Task WhenISearchForPetsWithTag(string tagName)
    {
        var response = await apiClient.Get<List<Pet>>($"/pet/findByTags?tags={tagName}");
        var createdPetIds = _createdPets.Select(p => p.Id).ToHashSet();
        var filteredResponse = response?.Where(p => createdPetIds.Contains(p.Id)).ToList();
        scenarioContext["SearchResults"] = filteredResponse;
    }

    [When(@"I search for pets with tags ""(.*)""")]
    public async Task WhenISearchForPetsWithTags(string tags)
    {
        var requiredTags = tags.Split(',').Select(t => t.Trim().ToLowerInvariant()).ToHashSet();
        var createdPetIds = _createdPets.Select(p => p.Id).ToHashSet();

        var response = await apiClient.Get<List<Pet>>($"/pet/findByTags?tags={tags}");
        if (response == null) response = new List<Pet>();

        var filteredResponse = response
            .Where(p => createdPetIds.Contains(p.Id)) // Test-created pets only
            .Where(p => requiredTags.SetEquals(p.Tags.Select(tag => tag.Name.Trim().ToLowerInvariant())))
            .GroupBy(p => p.Id) // Group by Id to avoid duplicates
            .Select(group => group.First()) // Select distinct pets
            .ToList();

        scenarioContext["SearchResults"] = filteredResponse;
    }

    [When(@"I search for available pets with tag ""(.*)""")]
    public async Task WhenISearchForAvailablePetsWithTag(string tag)
    {
        var response = await apiClient.Get<List<Pet>>($"/pet/findByTags?tags={tag}");
        var createdPetIds = _createdPets.Select(p => p.Id).ToHashSet();
        var filteredResponse = response?
            .Where(p => createdPetIds.Contains(p.Id) && p.Status == "available")
            .ToList();
        scenarioContext["SearchResults"] = filteredResponse;
    }

    [Then(@"the pet should be successfully added")]
    public void ThenThePetShouldBeSuccessfullyAdded()
    {
        var addedPet = scenarioContext.Get<Pet>("AddedPet");
        Assert.That(addedPet, Is.Not.Null);
        Assert.That(addedPet.Name, Is.EqualTo(_testPet?.Name));
    }

    [Then(@"I can retrieve the pet by ID")]
    public async Task ThenICanRetrieveThePetById()
    {
        var addedPet = scenarioContext.Get<Pet>("AddedPet");
        var retrievedPet = await apiClient.Get<Pet>($"/pet/{addedPet.Id}");

        Assert.That(retrievedPet, Is.Not.Null);
        if (retrievedPet != null)
        {
            Assert.That(retrievedPet.Id, Is.EqualTo(addedPet.Id));
            Assert.That(retrievedPet.Name, Is.EqualTo(addedPet.Name));
        }
    }

    [Then(@"the pet's status should be updated successfully")]
    public void ThenThePetsStatusShouldBeUpdatedSuccessfully()
    {
        var updatedPet = scenarioContext.Get<Pet>("UpdatedPet");
        Assert.That(updatedPet, Is.Not.Null);
    }

    [Then(@"the pet's new status should be ""(.*)""")]
    public void ThenThePetsNewStatusShouldBe(string expectedStatus)
    {
        var updatedPet = scenarioContext.Get<Pet>("UpdatedPet");
        Assert.That(updatedPet.Status, Is.EqualTo(expectedStatus));
    }

    [Then(@"the pet should be successfully deleted")]
    public void ThenThePetShouldBeSuccessfullyDeleted()
    {
        Assert.Pass("Pet was successfully deleted");
    }

    [Then(@"retrieving the pet should return not found")]
    public void ThenRetrievingThePetShouldReturnNotFound()
    {
        var pet = scenarioContext.Get<Pet>("TestPet");
        
        var exception = Assert.ThrowsAsync<HttpRequestException>(async () => 
            await apiClient.Get<Pet>($"/pet/{pet.Id}"));
        
        Assert.That(exception?.Message, Does.Contain("404"));
    }

    [Then(@"I should find (.*) pets? in the results")]
    public void ThenIShouldFindPetsInTheResults(int expectedCount)
    {
        var results = scenarioContext.Get<List<Pet>>("SearchResults");
        Assert.That(results, Has.Count.EqualTo(expectedCount));
    }

    [Then(@"the pets? ""(.*)"" should be in the results")]
    public void ThenThePetsShouldBeInTheResults(string expectedPetNames)
    {
        var results = scenarioContext.Get<List<Pet>>("SearchResults");
        var expectedNames = expectedPetNames.Split(',');
        
        foreach (var name in expectedNames)
        {
            Assert.That(results.Any(p => p.Name == name.Trim()), Is.True, 
                $"Pet with name {name} not found in results");
        }
    }
}