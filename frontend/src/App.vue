<script setup>
import { ref } from 'vue'
import LoginPage from './components/LoginPage.vue'
import ActivityTracker from './components/ActivityTracker.vue'

const loggedInUser = ref(null)

const handleLogin = (data) => {
  console.log('User logged in:', data)
  loggedInUser.value = data
}
</script>

<template>
  <div>
    <LoginPage v-if="!loggedInUser" @login-success="handleLogin" />
    <ActivityTracker v-else :user-id="loggedInUser.userId" />
  </div>
</template>
