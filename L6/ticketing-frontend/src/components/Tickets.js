import React, { useState, useEffect } from "react";
import hubConnection from "../signalr";  // Import po��czenia SignalR

const Tickets = () => {
    const [tickets, setTickets] = useState([]);

    useEffect(() => {
        // Pobierz wszystkie zg�oszenia z API na pocz�tku
        fetch("http://localhost:5177/api/tickets")
            .then((res) => res.json())
            .then(setTickets);

        // Nas�uchuj na nowe zg�oszenia
        hubConnection.on("TicketCreated", (ticket) => {
            setTickets((prev) => [...prev, ticket]);
        });

        // Nas�uchuj na zmiany w zg�oszeniu
        hubConnection.on("TicketUpdated", (updatedTicket) => {
            setTickets((prev) =>
                prev.map((ticket) => (ticket.id === updatedTicket.id ? updatedTicket : ticket))
            );
        });

        // Cleanup: usuni�cie nas�uchiwaczy podczas odmontowania komponentu
        return () => {
            hubConnection.off("TicketCreated");
            hubConnection.off("TicketUpdated");
        };
    }, []);

    return (
        <div>
            <h1>Lista Zg�osze�</h1>
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
