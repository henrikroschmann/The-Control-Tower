namespace The_Control_Tower.Models
{
    public interface IAirplane
    {
        /// <summary>
        /// Flight Code
        /// </summary>
        string FlightCode { get; set; }

        Status Statuses { get; set; }
        DocksEnum Dock { get; set; }
    }
}