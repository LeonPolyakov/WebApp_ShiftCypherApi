# WebApp_ShiftCypherApi
Please use Visual Studio 2019 to run this app
This app contains an API endpoint at http://localhost:23456/api/encode and is a Post 'verb'
The Headres section of the request must include Content-Type: application/json

Send the following json to the above endpoint:

{
  "Shift": 3,
  "Message": "the eagle has landed"
}