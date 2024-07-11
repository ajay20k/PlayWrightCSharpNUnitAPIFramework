Feature: UserCreation

@API @Ajay
Scenario: Verify GET Call Request With ​/BookStore/v1/Book?ISBN=9781449325862 Endpoint
	When I Send 'GET' Request to '/BookStore/v1/Book?ISBN=9781449325862' Endpoint of 'BookStoreBaseUrl'
	And I will verify the Status Code is 200
	Then I will verify Response is Contains Following details
		| isbn          | subTitle               | author               |
		| 9781449325862 | A Working Introduction | Richard E. Silverman |

@API @Ajay
Scenario: Verify POST Call Request With /Account/v1/GenerateToken Endpoint
	When I Send 'POST' Request to '/Account/v1/GenerateToken' Endpoint with 'BookStoreBaseUrl'
		| userName | password |
		| Aj       | Aj@12345 |
	And I will verify the Status Code is 200
	Then I will verify response info contains following details
		| status  |
		| Success |
