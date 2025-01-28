using SauceDemoUiBetsson.ApiPetStore.Helpers;
using SauceDemoUiBetsson.ApiPetStore.Models;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace SauceDemoUiBetsson.ApiPetStore.StepDefinitions;

[Binding]
public class StoreSteps(ApiClient apiClient, ScenarioContext scenarioContext)
{
    private StoreOrder? _testOrder;
    private Pet? _testPet;

    [Given(@"I have an existing pet in the store")]
    public async Task GivenIHaveAnExistingPetInTheStore()
    {
        // Create a test pet first
        _testPet = new Pet
        {
            Id = DateTime.Now.Ticks,
            Name = "Alex",
            Category = new Category { Id = 1, Name = "Alex" },
            Status = "available"
        };

        var response = await apiClient.Post<Pet>("/pet", _testPet);
        Assert.That(response, Is.Not.Null);
        scenarioContext["TestPet"] = response;
    }

    [When(@"I place an order with the following details")]
    public async Task WhenIPlaceAnOrderWithTheFollowingDetails(Table table)
    {
        var orderDetails = table.CreateInstance<StoreOrder>();
        var pet = scenarioContext.Get<Pet>("TestPet");

        _testOrder = new StoreOrder
        {
            Id = DateTime.Now.Ticks,
            PetId = pet.Id,
            Quantity = orderDetails.Quantity,
            ShipDate = DateTime.UtcNow.ToString("O"),
            Status = orderDetails.Status,
            Complete = orderDetails.Complete
        };

        var response = await apiClient.Post<StoreOrder>("/store/order", _testOrder);
        scenarioContext["TestOrder"] = response;
    }

    [Given(@"there is an existing order in the store")]
    public async Task GivenThereIsAnExistingOrderInTheStore()
    {
        await GivenIHaveAnExistingPetInTheStore();
        var pet = scenarioContext.Get<Pet>("TestPet");

        _testOrder = new StoreOrder
        {
            Id = DateTime.Now.Ticks,
            PetId = pet.Id,
            Quantity = 1,
            ShipDate = DateTime.UtcNow.ToString("O"),
            Status = "placed",
            Complete = false
        };

        var response = await apiClient.Post<StoreOrder>("/store/order", _testOrder);
        scenarioContext["TestOrder"] = response;
    }

    [When(@"I request the order by its ID")]
    public async Task WhenIRequestTheOrderByItsId()
    {
        var order = scenarioContext.Get<StoreOrder>("TestOrder");
        var response = await apiClient.Get<StoreOrder>($"/store/order/{order.Id}");
        scenarioContext["RetrievedOrder"] = response;
    }

    [When(@"I delete the order")]
    public async Task WhenIDeleteTheOrder()
    {
        var order = scenarioContext.Get<StoreOrder>("TestOrder");
        await apiClient.Delete<StoreOrder>($"/store/order/{order.Id}");
    }

    [Then(@"the order should be successfully placed")]
    public void ThenTheOrderShouldBeSuccessfullyPlaced()
    {
        var placedOrder = scenarioContext.Get<StoreOrder>("TestOrder");
        Assert.That(placedOrder, Is.Not.Null);
        Assert.That(placedOrder.Id, Is.EqualTo(_testOrder?.Id));
        Assert.That(placedOrder.Status, Is.EqualTo("placed"));
    }

    [Then(@"I can retrieve the order details")]
    public async Task ThenICanRetrieveTheOrderDetails()
    {
        var order = scenarioContext.Get<StoreOrder>("TestOrder");
        var retrievedOrder = await apiClient.Get<StoreOrder>($"/store/order/{order.Id}");
        
        Assert.That(retrievedOrder, Is.Not.Null);
        if (retrievedOrder != null)
        {
            Assert.That(retrievedOrder.Id, Is.EqualTo(order.Id));
            Assert.That(retrievedOrder.PetId, Is.EqualTo(order.PetId));
        }
    }

    [Then(@"the order details should be correctly returned")]
    public void ThenTheOrderDetailsShouldBeCorrectlyReturned()
    {
        var originalOrder = scenarioContext.Get<StoreOrder>("TestOrder");
        var retrievedOrder = scenarioContext.Get<StoreOrder>("RetrievedOrder");
        
        Assert.That(retrievedOrder, Is.Not.Null);
        Assert.That(retrievedOrder.Id, Is.EqualTo(originalOrder.Id));
        Assert.That(retrievedOrder.PetId, Is.EqualTo(originalOrder.PetId));
    }

    [Then(@"the order status should be ""(.*)""")]
    public void ThenTheOrderStatusShouldBe(string expectedStatus)
    {
        var retrievedOrder = scenarioContext.Get<StoreOrder>("RetrievedOrder");
        Assert.That(retrievedOrder.Status, Is.EqualTo(expectedStatus));
    }

    [Then(@"the order should be successfully deleted")]
    public void ThenTheOrderShouldBeSuccessfullyDeleted()
    {
        // The deletion itself serves as verification since it would throw an exception if unsuccessful
        Assert.Pass("Order was successfully deleted");
    }

    [Then(@"retrieving the order should return not found")]
    public void ThenRetrievingTheOrderShouldReturnNotFound()
    {
        var order = scenarioContext.Get<StoreOrder>("TestOrder");
        
        var exception = Assert.ThrowsAsync<HttpRequestException>(async () =>
            await apiClient.Get<StoreOrder>($"/store/order/{order.Id}"));
        
        Assert.That(exception?.Message, Does.Contain("404"));
    }
}