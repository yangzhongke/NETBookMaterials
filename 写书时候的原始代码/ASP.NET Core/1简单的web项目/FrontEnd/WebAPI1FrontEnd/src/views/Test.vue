<template>
  <div>
      <fieldset>
        <legend>Demo1</legend>
        <div>姓名：{{state.person.name}}</div>
        <div>{{state.person.isVIP?"VIP":"普通会员"}}</div>
        <div>注册时间：{{state.person.createdTime}}</div>        
      </fieldset>
      <fieldset>
        <legend>Search</legend>
        <input type="text" v-model="state.searchRequest.word"/>
        <select  v-model="state.searchRequest.siteName">
            <option value="Baidu">百度</option>
            <option value="Sogou">搜狗</option>
            <option value="360">360</option>
        </select>
        <input type="button" value="搜索" @click="searchClick" />        
      </fieldset>
  </div>
</template>

<script>
import axios from 'axios'
import {reactive,onMounted} from 'vue' 

export default {
  name: 'Test',
  setup(){    
    const state= reactive({person:{},searchRequest:{}});

    const searchClick=async ()=>{
      const payload = state.searchRequest;
      //如果提交js对象，那么默认就是json请求，content-type也是text/json，这样服务器端正好可以解析
      const resp = await axios.post('https://localhost:44360/api/Test/SubmitSearch',payload);
      const url = resp.data;
      location.href=url;
    };

    onMounted(async ()=>{
      const demo1 = await axios.get( 'https://localhost:44360/api/Test/Demo1');
      state.person = demo1.data;
    });
    return {state,searchClick};
  }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>

</style>
