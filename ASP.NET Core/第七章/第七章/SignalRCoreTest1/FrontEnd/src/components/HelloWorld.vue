<template>
    <input type="text"  v-model="state.userMessage" v-on:keypress="txtMsgOnkeypress"/>
    <div><ul>
        <li v-for="(msg,index) in state.messages" :key="index">{{msg}}</li>
    </ul></div>
</template>
<script>
    import { reactive, onMounted } from 'vue';
    import * as signalR from '@microsoft/signalr';
    let connection;
    export default {name: 'Login',
        setup() {
            const state = reactive({ userMessage: "", messages: [] });
            const txtMsgOnkeypress = async function (e) {
                if (e.keyCode != 13) return;
                try {
                    await connection.invoke("SendPublicMessage", state.userMessage);
                }catch (err) {
                    alert(err);
                    return;
                }
                state.userMessage = "";
            };
            onMounted(async function () {
                //const transport = signalR.HttpTransportType.WebSockets;
                const options = { skipNegotiation: true, transport: signalR.HttpTransportType.WebSockets  };
                connection = new signalR.HubConnectionBuilder()
                    .withUrl('https://localhost:7047/Hubs/ChatRoomHub', options)
                    //.withUrl('https://localhost:7047/Hubs/ChatRoomHub')
                    .withAutomaticReconnect()
                    .build();
                try {
                    await connection.start();
                }catch (err) {
                    alert(err);
                    return;
                }
                connection.on('ReceivePublicMessage', msg => {
                    state.messages.push(msg);
                });
            });
            return { state, txtMsgOnkeypress };
        },
    }
</script>
<style scoped>
</style>
