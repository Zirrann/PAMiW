import React, { useState, useEffect } from "react";
import "./App.css";
import { HubConnectionBuilder } from "@microsoft/signalr";

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
            .withUrl("http://localhost:5177/hubs/tickets") // Backend URL
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
        fetch("http://localhost:5177/api/tickets")
            .then((response) => response.json())
            .then((data) => setTickets(data));
    }, []);

    // Handling form data changes
    const handleChange = (e) => {
        const { name, value } = e.target;
        setNewTicket((prevState) => ({
            ...prevState,
            [name]: value,
        }));
    };

    // Function to handle form submission
    const handleSubmit = async (e) => {
        e.preventDefault();

        // Get the current date for createdAt and updatedAt
        const currentDate = new Date().toISOString();

        // Prepare the ticket data with the correct structure
        const ticketData = {
            id: 0, // id is 0 for a new ticket
            title: newTicket.title,
            description: newTicket.description,
            status: "Open", // Default status
            priority: newTicket.priority,
            createdAt: currentDate,
            updatedAt: currentDate,
            comments: [] // Initialize comments as an empty array
        };

        // Send the ticket data to the backend
        const response = await fetch("http://localhost:5177/api/tickets", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(ticketData),
        });

        // If the ticket was successfully added, reset the form
        if (response.ok) {
            setNewTicket({
                title: "",
                description: "",
                priority: "Medium",
            });
        }
    };

    return (
        <div className="App">
            <h1>Ticket Management System</h1>

            <div>
                <h2>Add a New Ticket</h2>
                <form onSubmit={handleSubmit}>
                    <div>
                        <label htmlFor="title">Title</label>
                        <input
                            type="text"
                            id="title"
                            name="title"
                            value={newTicket.title}
                            onChange={handleChange}
                            required
                        />
                    </div>

                    <div>
                        <label htmlFor="description">Description</label>
                        <textarea
                            id="description"
                            name="description"
                            value={newTicket.description}
                            onChange={handleChange}
                            required
                        />
                    </div>

                    <div>
                        <label htmlFor="priority">Priority</label>
                        <select
                            id="priority"
                            name="priority"
                            value={newTicket.priority}
                            onChange={handleChange}
                        >
                            <option value="Low">Low</option>
                            <option value="Medium">Medium</option>
                            <option value="High">High</option>
                        </select>
                    </div>

                    <button type="submit">Add Ticket</button>
                </form>
            </div>

            <div>
                <h2>Ticket List</h2>
                <ul>
                    {tickets.map((ticket) => (
                        <li key={ticket.id}>
                            <h3>{ticket.title}</h3>
                            <p>{ticket.description}</p>
                            <p>Status: {ticket.status}</p>
                            <p>Priority: {ticket.priority}</p>
                        </li>
                    ))}
                </ul>
            </div>
        </div>
    );
}

export default App;
