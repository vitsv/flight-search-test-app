<template>
  <v-card elevation="1" rounded="lg" color="primary" variant="tonal">
    <v-card-title class="pa-4 pb-2">
      <v-icon icon="mdi-check-circle-outline" class="mr-2" />
      Search Summary
    </v-card-title>
    <v-card-text class="pa-4 pt-0">
      <v-list bg-color="transparent" density="compact">
        <v-list-item>
          <template #prepend><v-icon icon="mdi-swap-horizontal" /></template>
          <v-list-item-title>Trip type</v-list-item-title>
          <v-list-item-subtitle>{{ tripTypeLabel }}</v-list-item-subtitle>
        </v-list-item>

        <v-list-item>
          <template #prepend><v-icon icon="mdi-airplane-takeoff" /></template>
          <v-list-item-title>From</v-list-item-title>
          <v-list-item-subtitle>{{ result.origin?.label }}</v-list-item-subtitle>
        </v-list-item>

        <v-list-item>
          <template #prepend><v-icon icon="mdi-airplane-landing" /></template>
          <v-list-item-title>To</v-list-item-title>
          <v-list-item-subtitle>{{ result.destination?.label }}</v-list-item-subtitle>
        </v-list-item>

        <v-list-item v-if="result.departureDate">
          <template #prepend><v-icon icon="mdi-calendar" /></template>
          <v-list-item-title>Departure</v-list-item-title>
          <v-list-item-subtitle>{{ formatDate(result.departureDate) }}</v-list-item-subtitle>
        </v-list-item>

        <v-list-item v-if="result.returnDate">
          <template #prepend><v-icon icon="mdi-calendar-arrow-right" /></template>
          <v-list-item-title>Return</v-list-item-title>
          <v-list-item-subtitle>{{ formatDate(result.returnDate) }}</v-list-item-subtitle>
        </v-list-item>

        <v-list-item>
          <template #prepend><v-icon icon="mdi-account-group" /></template>
          <v-list-item-title>Passengers</v-list-item-title>
          <v-list-item-subtitle>
            {{ result.passengers }} {{ result.passengers === 1 ? 'passenger' : 'passengers' }}
          </v-list-item-subtitle>
        </v-list-item>
      </v-list>
    </v-card-text>
  </v-card>
</template>

<script setup>
import { computed } from 'vue'

const props = defineProps({
  result: { type: Object, required: true },
})

const tripTypeLabel = computed(() =>
  props.result.tripType === 'return' ? 'Return' : 'One-way'
)

function formatDate(date) {
  return new Date(date).toLocaleDateString('en-GB', {
    day: 'numeric',
    month: 'short',
    year: 'numeric',
  })
}
</script>
