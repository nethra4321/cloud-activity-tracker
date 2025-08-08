<script setup>
import { ref } from 'vue'
import CryptoJS from 'crypto-js'
import axios from 'axios'

const email = ref('')
const username = ref('')
const password = ref('')

const emit = defineEmits(['login-success'])

const login = () => {
  if (!username.value || !password.value || !email.value) {
    alert('Please enter your username, email, and password.')
    return
  }

  const encryptedPassword = CryptoJS.SHA256(password.value).toString()

  axios.post('http://localhost:5000/api/activity', {
    eventType: 'UserLogin',
    timestamp: new Date().toISOString(),
    userId: username.value,
    email: email.value,
    encryptedPassword
  })
  .then(res => {
    console.log('UserLogin sent to backend:', res.data)
    emit('login-success', {
      email: email.value,
      userId: username.value,
      encryptedPassword
    })
  })
  .catch(err => {
    console.error('Login event POST failed:', err)
    alert('Failed to login or send data.')
  })
}
</script>

<template>
  <div>
    <h2>Login</h2>
    <input v-model="username" placeholder="Username" />
    <input v-model="email" placeholder="Email" />
    <input v-model="password" type="password" placeholder="Password" />
    <button @click="login">Login</button>
  </div>
</template>

<style scoped>
input {
  display: block;
  margin: 8px 0;
  padding: 6px;
  width: 200px;
}
button {
  padding: 6px 12px;
  cursor: pointer;
}
</style>
