import React, { useState, useEffect } from "react";
import hubConnection from "../signalr";  // Import połączenia SignalR

const Tickets = () => {
    const [tickets, setTickets] = useState([]);

    useEffect(() => {
        // Pobierz wszystkie zgłoszenia z API na początku
        fetch("http://localhost:5177/api/tickets")
            .then((res) => res.json())
            .then(setTickets);

        // Nasłuchuj na nowe zgłoszenia
        hubConnection.on("TicketCreated", (ticket) => {
            setTickets((prev) => [...prev, ticket]);
        });

        // Nasłuchuj na zmiany w zgłoszeniu
        hubConnection.on("TicketUpdated", (updatedTicket) => {
            setTickets((prev) =>
                prev.map((ticket) => (ticket.id === updatedTicket.id ? updatedTicket : ticket))
            );
        });

        // Cleanup: usunięcie nasłuchiwaczy podczas odmontowania komponentu
        return () => {
            hubConnection.off("TicketCreated");
            hubConnection.off("TicketUpdated");
        };
    }, []);

    return (
        <div>
            <h1>Lista Zgłoszeń</h1>
            <ul>
                {tickets.map((ticket) => (
                    <li key={ticket.id}>
                        {ticket.title} - {ticket.status}
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default Tickets;
