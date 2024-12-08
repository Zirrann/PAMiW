import React, { useState, useEffect } from "react";
import { HubConnectionBuilder } from "@microsoft/signalr";
import "./App.css";
import TicketForm from "./components/TicketForm";
import TicketList from "./components/TicketList";

function App() {
    const [tickets, setTickets] = useState([]);
    const [hubConnection, setHubConnection] = useState(null);
    const [newTicket, setNewTicket] = useState({
        title: "",
        description: "",
        priority: "Medium",
    });

    // SignalR connection
    useEffect(() => {
        const connection = new HubConnectionBuilder()
            .withUrl("http://localhost:5000/hubs/tickets")
            .withAutomaticReconnect()
            .build();

        connection.start().catch((err) => console.error("SignalR error: ", err));

        connection.on("TicketCreated", (ticket) => {
            setTickets((prevTickets) => [...prevTickets, ticket]);
        });

        connection.on("TicketUpdated", (updatedTicket) => {
            setTickets((prevTickets) =>
                prevTickets.map((ticket) =>
                    ticket.id === updatedTicket.id ? updatedTicket : ticket
                )
            );
        });

        setHubConnection(connection);

        return () => {
            connection.stop();
        };
    }, []);

    // Fetch existing tickets
    useEffect(() => {
        fetch("http://localhost:5000/api/tickets")
            .then((response) => response.json())
            .then((data) => setTickets(data));
    }, []);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setNewTicket((prevState) => ({
            ...prevState,
            [name]: value,
        }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        if (newTicket.id) {
            const response = await fetch(`http://localhost:5000/api/tickets/${newTicket.id}`, {
                method: "PUT",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(newTicket),
            });

            if (response.ok) {
                setNewTicket({
                    title: "",
                    description: "",
                    priority: "Medium",
                });
            }
        } else {
            const currentDate = new Date().toISOString();

            const ticketData = {
                id: 0,
                title: newTicket.title,
                description: newTicket.description,
                status: "Open",
                priority: newTicket.priority,
                createdAt: currentDate,
                updatedAt: currentDate,
                comments: [],
            };

            const response = await fetch("http://localhost:5000/api/tickets", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(ticketData),
            });

            if (response.ok) {
                setNewTicket({
                    title: "",
                    description: "",
                    priority: "Medium",
                });
            }
        }
    };

    const handleEdit = (ticket) => {
        setNewTicket({
            id: ticket.id,
            title: ticket.title,
            description: ticket.description,
            priority: ticket.priority,
        });
    };

    return (
        <div className="App">
            <h1>Ticket Management System</h1>;
            <TicketForm
                newTicket={newTicket}
                handleChange={handleChange}
                handleSubmit={handleSubmit}
            />
            <h2>Ticket List</h2>
            <TicketList tickets={tickets} onEdit={handleEdit} />
        </div>
    );
}

export default App;
