<template>
    <div>
        <input type="button" value="导入" v-on:click="importData"/>
        <div>{{state.message}}</div>
        <progress :value="state.progress.value" :max="state.progress.total"></progress>
    </div>
</template>
<script>
    import { reactive, onMounted } from 'vue';
    import * as signalR from '@microsoft/signalr';
    let connection;
    export default {name: 'Hello',
        setup() {
            const state = reactive({ progress: {value:0,total:1},message:""});
            onMounted(async function () {
                connection = new signalR.HubConnectionBuilder()
                    .withUrl('https://localhost:7055/Hubs/ImportDictHub')
                    .withAutomaticReconnect().build();
                try {
                    await connection.start();
                } catch (err) {
                    alert(err);
                    return;
                }
                connection.on('ImportProgress', (value,total) => {
                    state.progress.value = value;
                    state.progress.total = total;
                    state.message = "导入中(" + value + "/" + total+")";
                });
                connection.on('Started', () => {
                    state.message = "文件解析完毕，开始导入";
                });
                connection.on('Failed', () => {
                    state.message = "导入失败";
                });
                connection.on('Completed', () => {
                    state.message = "导入完成";
                });
            });
            const importData = async function () {
                try {
                    await connection.invoke("Import");
                    state.message = "开始导入";
                }
                catch (err) {
                    alert(err);
                }
            };
            return { state, importData };
        },
    }
</script>
<style scoped>
</style>
