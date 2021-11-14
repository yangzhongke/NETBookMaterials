<template>
    <fieldset>
        <legend>登录</legend>
        <div>
            用户名：<input type="text" v-model="state.loginData.userName"/>
        </div>
        <div>
            密码：<input type="password"  v-model="state.loginData.password">
        </div>
        <div>
            <input type="button" value="登录" v-on:click="loginClick"/>
        </div>
    </fieldset>
    公屏：<input type="text" v-model="state.userMessage" v-on:keypress="txtMsgOnkeypress" />
    <div>
        私聊给<input type="text" v-model="state.privateMsg.destUserName"/>
        说<input type="text" v-model="state.privateMsg.message"
                v-on:keypress="txtPrivateMsgOnkeypress"/>
    </div>
    <div>
        <ul>
            <li v-for="(msg,index) in state.messages" :key="index">{{msg}}</li>
        </ul>
    </div>
</template>
<script>
    import { reactive, onMounted } from 'vue';
    import * as signalR from '@microsoft/signalr';
    import axios from 'axios';
    let connection;
    export default {name: 'Login',
        setup() {
            const state = reactive({accessToken:"",userMessage: "", messages: [],
                loginData: { userName: "", password: "" },
                privateMsg: { destUserName:"",message:""},
            });
            const startConn = async function () {
                const transport = signalR.HttpTransportType.WebSockets;
                const options = { skipNegotiation: true, transport: transport };
                options.accessTokenFactory = () => state.accessToken;
                connection = new signalR.HubConnectionBuilder()
                    .withUrl('https://localhost:7173/Hubs/ChatRoomHub', options)
                    .withAutomaticReconnect().build();
                try {
                    await connection.start();
                } catch (err) {
                    alert(err);
                    return;
                }
                connection.on('ReceivePublicMessage', msg => {
                    state.messages.push(msg);
                });
                connection.on('ReceivePrivateMessage', (srcUser,time,msg) => {
                    state.messages.push(srcUser+"在"+time+"发来私信:"+msg);
                });
                connection.on('UserAdded', userName => {
                    state.messages.push("系统消息：欢迎" + userName+"加入我们!");
                });
                alert("登陆成功可以聊天了");
            };
            const loginClick = async function () {
                const resp = await axios.post('https://localhost:7173/Test1/Login',
                    state.loginData);
                state.accessToken = resp.data;
                startConn();
            };
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
            const txtPrivateMsgOnkeypress = async function (e) {
                if (e.keyCode != 13) return;
                const destUserName = state.privateMsg.destUserName;
                const msg = state.privateMsg.message;
                try {
                    const ret = await connection.invoke("SendPrivateMessage", destUserName, msg);
                    if (ret != "ok") { alert(ret);};
                } catch (err) {
                    alert(err);
                    return;
                }
                state.privateMsg.message = "";
            };
            return { state, loginClick, txtMsgOnkeypress, txtPrivateMsgOnkeypress };
        },
    }
</script>
<style scoped>
</style>
