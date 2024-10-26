Feature: Home
	Check if home functionality works


@mytag
Scenario: Home 1
	Given I navigate to application
	And I click the Login link

@mytag
Scenario: Home 2
	Given I navigate to application
	And I click the Login link
	And I enter username and password
		| UserName | Password |
		| admin    | password |

