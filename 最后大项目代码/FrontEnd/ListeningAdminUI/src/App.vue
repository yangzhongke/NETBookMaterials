<template>
  <div>
    <router-link to="/Login">登录</router-link>
    <span>|</span>
    <router-link to="/AdminUserList">管理员管理</router-link>
    <span>|</span>
    <router-link to="/CategoryList">听力管理</router-link>
  </div>
  <router-view />  
</template>
<style lang="css">
@import "styles/global.css";
</style>