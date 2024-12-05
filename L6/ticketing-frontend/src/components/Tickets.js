import React, { useState, useEffect } from "react";
import hubConnection from "../signalr";  // Import po³¹czenia SignalR

const Tickets = () => {
    const [tickets, setTickets] = useState([]);

    useEffect(() => {
        // Pobierz wszystkie zg³oszenia z API na pocz¹tku
        fetch("http://localhost:5177/api/tickets")
            .then((res) => res.json())
            .then(setTickets);

        // Nas³uchuj na nowe zg³oszenia
        hubConnection.on("TicketCreated", (ticket) => {
            setTickets((prev) => [...prev, ticket]);
        });

        // Nas³uchuj na zmiany w zg³oszeniu
        hubConnection.on("TicketUpdated", (updatedTicket) => {
            setTickets((prev) =>
                prev.map((ticket) => (ticket.id === updatedTicket.id ? updatedTicket : ticket))
            );
        });

        // Cleanup: usuniêcie nas³uchiwaczy podczas odmontowania komponentu
        return () => {
            hubConnection.off("TicketCreated");
            hubConnection.off("TicketUpdated");
        };
    }, []);

    return (
        <div>
            <h1>Lista Zg³oszeñ</h1>
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
