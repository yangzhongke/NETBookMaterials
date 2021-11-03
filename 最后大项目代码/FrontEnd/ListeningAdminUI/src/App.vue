<template>
  <div>
	<router-link to="/Login">登录</router-link>
	<span>|</span>
	<router-link to="/AdminUserList">管理员管理</router-link>
	<span>|</span>
	<router-link to="/CategoryList">听力管理</router-link>
  </div>
  <div style="margin-bottom: 30px;"></div>
	<router-view />  
</template>
<script>

</script>
<style lang="css">
@import "styles/global.css";
</style>
<style scoped>
</style>