<template>
  <div>
      <fieldset>
        <legend>Login</legend><label for="userName">用户名:</label>
        <input type="text" v-model="state.loginData.userName" id="userName" />
        <label for="password">密码:</label>
        <input type="password" v-model="state.loginData.password" id="password" />
        <input type="submit" value="登录" @click="loginSubmit"/>    
      </fieldset>
      <table v-if="state.processes.length>0">
          <thead>
              <tr><th style="width:50px;">进程Id</th><th style="width:20px;">进程名</th><th style="width:20px;">内存占用</th></tr>
          </thead>
          <tbody>
            <tr v-for="p in state.processes" :key="p.id">
                <td>{{p.id}}</td><td>{{p.processName}}</td>
                <td>{{(p.workingSet64/1024)}}KB</td>
            </tr>
          </tbody>
      </table>
  </div>
</template>
<script>
import axios from 'axios';
import {reactive,onMounted} from 'vue' 
export default {
  name: 'Login',
  setup(){
    const state=reactive({loginData:{},processes:[]});
    const loginSubmit=async ()=>{
      const payload = state.loginData;
      //如果提交js对象，那么默认就是json请求，content-type也是text/json，这样服务器端正好可以解析
      const resp = await axios.post('https://localhost:44360/api/Login/Login',payload);
      const data = resp.data;
      if(!data.isOK)
      {
          alert("登录失败");
          return;
      }
      state.processes = data.processes;
    }
	  return {state,loginSubmit};
  },
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>

</style>
