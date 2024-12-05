import * as signalR from '@microsoft/signalr';

const hubConnection = new signalR.HubConnectionBuilder()
    .withUrl('http://localhost:5177/hubs/tickets')
    .withAutomaticReconnect()
    .build();

hubConnection.start().catch(err => console.error(err));

export default hubConnection;
