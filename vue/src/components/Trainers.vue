<script setup lang="ts">
import { onMounted, ref } from 'vue';

const trainers = ref();
onMounted(async () => {
  await fetchTrainers();
});
async function fetchTrainers() {
  try {
    const response = await fetch('https://localhost:7221/api/trainers');
    trainers.value = await response.json();
  } catch (err) {
    console.error(err);
  }
}

const name = ref('');
const region = ref('');
const createTrainerPending = ref(false);
async function createTrainer() {
    createTrainerPending.value = true;

    try {
        await fetch('https://localhost:7221/api/trainers', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ Name: name.value , Region: region.value }),
        });
        name.value = '';
        region.value = '';
        await fetchTrainers();
    } catch (err) {
        console.error(err);
    } finally {
        createTrainerPending.value = false;
    }
}
</script>
<template>
  <div class="trainers-container">
    <h2>Pokémon Trainers</h2>
    
    <form @submit.prevent="createTrainer" class="trainer-form">
      <input type="text" placeholder="Trainer Name" v-model="name" required />
      <input type="text" placeholder="Region" v-model="region" required />
      <button :disabled="createTrainerPending" type="submit">
        {{ createTrainerPending ? 'Adding...' : 'Add Trainer' }}
      </button>
    </form>

    <button @click="fetchTrainers" class="refresh-btn">Refresh Trainers</button>

    <div class="trainers-list">
      <div v-if="trainers && trainers.length > 0">
        <div v-for="trainer in trainers" :key="trainer.id" class="trainer-card">
          <h3>{{ trainer.name }}</h3>
          <p>Region: {{ trainer.region }}</p>
        </div>
      </div>
      <p v-else-if="trainers && trainers.length === 0" class="no-trainers">No trainers yet. Add one!</p>
      <p v-else class="loading">Loading...</p>
    </div>
  </div>
</template>

<style scoped>
.trainers-container {
  max-width: 600px;
  margin: 0 auto;
  padding: 20px;
}

h2 {
  color: #2c3e50;
  margin-bottom: 20px;
}

.trainer-form {
  display: flex;
  gap: 10px;
  margin-bottom: 15px;
}

.trainer-form input {
  flex: 1;
  padding: 10px;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 14px;
}

.trainer-form input:focus {
  outline: none;
  border-color: #42b983;
}

.trainer-form button {
  padding: 10px 20px;
  background-color: #42b983;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 14px;
  transition: background-color 0.2s;
}

.trainer-form button:hover:not(:disabled) {
  background-color: #359268;
}

.trainer-form button:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.refresh-btn {
  padding: 8px 16px;
  background-color: #646cff;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 14px;
  margin-bottom: 20px;
  transition: background-color 0.2s;
}

.refresh-btn:hover {
  background-color: #535bf2;
}

.trainers-list {
  margin-top: 20px;
}

.trainer-card {
  background-color: #f9f9f9;
  border: 1px solid #e0e0e0;
  border-radius: 8px;
  padding: 15px;
  margin-bottom: 10px;
}

.trainer-card h3 {
  margin: 0 0 8px 0;
  color: #2c3e50;
  font-size: 18px;
}

.trainer-card p {
  margin: 0;
  color: #666;
  font-size: 14px;
}

.no-trainers,
.loading {
  text-align: center;
  color: #999;
  padding: 20px;
  font-style: italic;
}
</style>