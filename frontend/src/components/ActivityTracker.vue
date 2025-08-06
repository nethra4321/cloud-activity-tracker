<template>
  <div>
    <h2>Activity Tracker</h2>
    <p>Tracking activity in this tab...</p>
  </div>
</template>

<script setup>
import { onMounted, onUnmounted } from 'vue'
import axios from 'axios'

let activityTimeout

const sendActivity = (eventType) => {
  axios.post('http://localhost:5000/api/activity', {
    eventType,
    timestamp: new Date().toISOString(),
    userId: 'user123'
  }).then(res => {
   console.log(`Sent: ${eventType}`, res.data)
  }).catch(err => {
    console.error(`Activity POST failed:`, err)
  });
}


const handleMouseMove = () => {
  sendActivity('mouse_move')
}

const handleVisibilityChange = () => {
  const eventType = document.hidden ? 'tab_hidden' : 'tab_visible'
  sendActivity(eventType)
}

onMounted(() => {
  document.addEventListener('mousemove', handleMouseMove)
  document.addEventListener('visibilitychange', handleVisibilityChange)

  // periodic ping
  // activityTimeout = setInterval(() => sendActivity('heartbeat'), 10000)
})

onUnmounted(() => {
  document.removeEventListener('mousemove', handleMouseMove)
  document.removeEventListener('visibilitychange', handleVisibilityChange)
  clearInterval(activityTimeout)
})
</script>