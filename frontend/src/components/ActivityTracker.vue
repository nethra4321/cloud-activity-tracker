<script setup>
import { onMounted, onUnmounted } from 'vue'
import { defineProps } from 'vue'
import axios from 'axios'

const props = defineProps({
  userId: String
})

let activityTimeout

const sendActivity = (eventType) => {
  axios.post('http://51.20.140.225:5000/api/activity', {
    eventType,
    timestamp: new Date().toISOString(),
    userId: props.userId
  }).then(res => {
    console.log(`Sent: ${eventType}`, res.data)
  }).catch(err => {
    console.error(`Activity POST failed:`, err)
  })
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
})

onUnmounted(() => {
  document.removeEventListener('mousemove', handleMouseMove)
  document.removeEventListener('visibilitychange', handleVisibilityChange)
  clearInterval(activityTimeout)
})
</script>

<template>
  <div>
    <h2>Activity Tracker</h2>
    <p>Tracking for user: {{ userId }}</p>
  </div>
</template>
