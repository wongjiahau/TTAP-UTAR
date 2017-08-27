# Cases
The cases are written in order such that the next cases also satisfy the complements of all the previous cases.   
For example, let say case 1 is "Dog is hungry" and case 2 is "Dog is barking".   
So case 2 actually means "Dog is NOT hungry and dog is barking".  

| No. | Cases                                                                                       | Expected outcome                                                                                                    |
|-----|---------------------------------------------------------------------------------------------|---------------------------------------------------------------------------------------------------------------------|
| 1   | No Internet connection                                                                      | Display message showing no Internet connection and allow user to Retry.                                             |
| 2   | Cannot load Login page                                                                      | Display message showing no Internet connection and allow user to Retry.                                             |
| 3   | Login failure because user keyed in invalid information (ID/Password/Captcha).              | Display error message indicating login failed.                                                                      |
| 4   | No data is available at course time table preview.                                          | Display message indicating no data available.                                                                       |
| 5   | After slots is loaded, user click Back button (which will navigate back to the login page). | Display dialogbox asking user whethere he wants to login again because this action will override the previous data. |
|     |                                                                                             |                                                                                                                     |
|     |                                                                                             |                                                                                                                     |
|     |                                                                                             |                                                                                                                     |
|     |                                                                                             |                                                                                                                     |
|     |                                                                                             |                                                                                                                     |
