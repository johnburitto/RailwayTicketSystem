# Description
**RailwayTicketSystem** is a computer system designed to sell train tickets. Its main goal is to simplify and automate the process of purchasing tickets for passengers.

This system enables passengers to order train tickets quickly and conveniently. It offers a large database of train timetables, available routes, types of carriages and available seats.

With the **RailwayTicketSystem**, users can view train timetables, select the desired route, class of carriage and seat, and make payments using various payment methods.

The system also provides the possibility of user registration. Viewing purchased tickets and sending them to your mail.

# Functionality
### User
* Login in system
* Register in system
* Watch all current avaliable tickets
* Fastbook ticket
* Book ticket by place
* Search by to/from city
* Search by departure/arrival time
* Price filtering
* Return ticket
* Cart
* Get ticket to e-mail (Optional)
* Rate train car (Optional)
* Search by train car rating (Optional)

### Admin
* Login in system
* Create other users
* Access to all tables and data
* Full data manipulation accessibility

# Diagrams
**There are early, raw and abstract diagrams. They may be changed!!!**

## UseCase diagram
![image](https://github.com/johnburitto/RailwayTicketSystem/assets/79087305/c52a12ae-185d-4bc5-ae96-fe127d6c16fd)
![image](https://github.com/johnburitto/RailwayTicketSystem/assets/79087305/f030f1ab-8212-46f5-91f4-cdca33f77a94)

## Class diagram
Not final, but conceptually close to the release class diagram.
**UPDATE** Train have aggregation on Route, not Ticket.

![image](https://github.com/johnburitto/RailwayTicketSystem/assets/79087305/4fb7e2e4-afba-47a1-909e-d39f2f38e5f8)

Also create(has all data witout Audit metadata and id) and read(has all data witout Audit metadata) Dtos.

![image](https://github.com/johnburitto/RailwayTicketSystem/assets/79087305/ae438b09-da9c-4bb0-a93f-51facd48d394)

So far, only CRUD services are presented in the diagram, then they will be supplemented with the necessary methods. Controllers will also be presented here, an example of the controller API is below.

![image](https://github.com/johnburitto/RailwayTicketSystem/assets/79087305/562cf7d6-9dbb-4ac4-9797-661c6a27f171)

# Development
Only an abstract and incomplete version of the project is described here, but from what has been said you can imagine the further development and things to expect in the final product. Further functionality will be described here.
