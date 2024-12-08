import React from "react";
import TicketItem from "./TicketItem";

function TicketList({ tickets, onEdit }) {
    return (
        <ul>
            {tickets.map((ticket) => (
                <TicketItem key={ticket.id} ticket={ticket} onEdit={onEdit} />
            ))}
        </ul>
    );
}

export default TicketList;
