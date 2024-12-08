import React from "react";

function TicketItem({ ticket, onEdit }) {
    return (
        <li>
            <h3>{ticket.title}</h3>
            <p>{ticket.description}</p>
            <p>Status: {ticket.status}</p>
            <p>Priority: {ticket.priority}</p>
            <button onClick={() => onEdit(ticket)}>Edytuj</button>
        </li>
    );
}

export default TicketItem;
