import React from "react";

function TicketForm({ newTicket, handleChange, handleSubmit }) {
    return (
        <div>
            <h2>{newTicket.id ? "Edit Ticket" : "Add a New Ticket"}</h2>
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

                <button type="submit">
                    {newTicket.id ? "Save changes" : "Add ticket"}
                </button>
            </form>
        </div>
    );
}

export default TicketForm;
