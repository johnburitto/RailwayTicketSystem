using Core.Entities;

namespace Extensions
{
    public static class TicketExtensions
    {
        public static string ToHTMLForm(this Ticket? ticket)
        {
            return $"<div>" +
                   $"   <ul style=\"list-style-type: none; padding: 0; margin: 0;\">" +
                   $"       <li><span>Id Користувача: </span>{ticket?.UserId}</li>" +
                   $"       <li><span>Час бронювання: </span>{ticket?.BookDate}</li>" +
                   $"       <li><span>Ціна: </span>{ticket?.Place.Price}</li>" +
                   $"       <li><span>Місце: </span>{ticket?.Place.Number}</li>" +
                   $"       <li><span>Вагон: </span>{ticket?.Place.TrainCar.Number}</li>" +
                   $"       <li><span>Потяг: </span>{ticket?.Place.TrainCar.Train.Number}</li>" +
                   $"       <li><span>Маршрут: </span>{ticket?.Place.TrainCar.Train.Route.FromCity} - {ticket?.Place.TrainCar.Train.Route.ToCity}</li>" +
                   $"       <li><span>Час: </span>{ticket?.Place.TrainCar.Train.Route.DepartureTime} - {ticket?.Place.TrainCar.Train.Route.ArrivalTime}</li>" +
                   $"       <li><span>Час в дорозі: </span>{ticket?.Place.TrainCar.Train.Route.TravelTime}</li>" +
                   $"   </ul>" +
                   $"</div>";
        }
    }
}
