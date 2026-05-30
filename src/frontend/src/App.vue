<template>
  <v-app>
    <v-main>
      <v-container class="py-8" max-width="720">
        <FlightSearchForm />
      </v-container>
    </v-main>
  </v-app>
</template>

<script setup>
import FlightSearchForm from './components/FlightSearchForm.vue'
</script>
