<template>
<div>
  <el-button-group>
    <el-button type="primary" icon="el-icon-circle-plus-outline" @click="addnew">创建管理员</el-button>
  </el-button-group> 
  <el-table row-key='id'
    :data="state.users"
    style="width: 100%">  
    <el-table-column width="80px" prop="userName" label="用户名"></el-table-column>
    <el-table-column prop="phoneNumber" label="手机号" width="120px" ></el-table-column>
    <el-table-column prop="creationTime" label="创建时间" width="160px"></el-table-column>
    <el-table-column fixed="right" label="操作">
      <template #default="scope">
        <el-button type="text" size="small">
          <button @click="deleteItem(scope.row)">删除</button>
        </el-button>
        <el-button type="text" size="small">
          <button @click="edit(scope.row.id)">修改</button>
        </el-button>
        <el-button type="text" size="small">
          <button @click="resetPassword(scope.row.id)">重置密码</button>
        </el-button>
      </template>
    </el-table-column>    
  </el-table>    
</div>
</template>

<script>
import axios from 'axios';
import {reactive,onMounted, getCurrentInstance} from 'vue' 
import {useRouter } from 'vue-router'

export default {
  name: 'AdminUserList',
  setup(){
    const router = useRouter();
    const {apiRoot} = getCurrentInstance().proxy;
    const state=reactive({users:[]});    
    onMounted(async function(){
      const {data}=await axios.get(`${apiRoot}/IdentityService/UserAdmin/FindAllUsers`);      
      state.users = data;
    });
    const deleteItem=async (user)=>{      
      const id = user.id;
      const name = user.userName;
      if(!confirm(`真的要删除${name}吗？`))
        return;
      await axios.delete(`${apiRoot}/IdentityService/UserAdmin/DeleteAdminUser/${id}`);      
      state.users = state.users.filter(e=>e.id!=id);//刷新表格
    };

    const addnew=()=>{
      router.push({name:'AdminUserAdd'});
    };
    const edit=(id)=>{
      router.push({name:'AdminUserEdit',query:{id:id}});
    };
    const resetPassword=async(id)=>{
      await axios.post(`${apiRoot}/IdentityService/UserAdmin/ResetAdminUserPassword/${id}`); 
      alert("密码已经重置，新密码已经发到用户的手机号");   
      router.push({name:'AdminUserList'});
    };
	  return {state,deleteItem,addnew,edit,resetPassword};
  },
}
</script>