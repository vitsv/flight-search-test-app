<template>
  <v-card elevation="3" rounded="lg">
    <v-card-title class="text-h5 pa-6 pb-2">
      <v-icon icon="mdi-airplane" class="mr-2" />
      Flight Search
    </v-card-title>

    <v-card-text class="pa-6 pt-4">
      <!-- Trip type toggle -->
      <v-btn-toggle
        v-model="tripType"
        mandatory
        rounded="pill"
        color="primary"
        density="comfortable"
        class="mb-6"
      >
        <v-btn value="one-way" prepend-icon="mdi-arrow-right">One-way</v-btn>
        <v-btn value="return" prepend-icon="mdi-arrow-left-right">Return</v-btn>
      </v-btn-toggle>

      <v-row>
        <!-- Origin -->
        <v-col cols="12" sm="6">
          <v-autocomplete
            v-model="origin"
            :items="origins"
            item-title="label"
            item-value="code"
            label="From"
            prepend-inner-icon="mdi-airplane-takeoff"
            :loading="loadingOrigins"
            :error-messages="originsError"
            clearable
            no-data-text="No airports available"
            @update:model-value="onOriginChange"
          />
        </v-col>

        <!-- Destination -->
        <v-col cols="12" sm="6">
          <v-autocomplete
            v-model="destination"
            :items="destinations"
            item-title="label"
            item-value="code"
            label="To"
            prepend-inner-icon="mdi-airplane-landing"
            :loading="loadingDestinations"
            :error-messages="destinationsError"
            :disabled="!origin || loadingDestinations"
            clearable
            no-data-text="No destinations found"
          />
        </v-col>
      </v-row>

      <v-row class="mt-2">
        <v-col cols="12">
          <v-btn
            color="primary"
            size="large"
            block
            :disabled="!origin || !destination"
            prepend-icon="mdi-magnify"
            @click="onSearch"
          >
            Search Flights
          </v-btn>
        </v-col>
      </v-row>
    </v-card-text>
  </v-card>

  <!-- Search result summary -->
  <SearchSummary v-if="searchResult" :result="searchResult" class="mt-6" />
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { fetchOrigins, fetchDestinations } from '../api.js'
import SearchSummary from './SearchSummary.vue'

const tripType = ref('one-way')
const origin = ref(null)
const destination = ref(null)
const searchResult = ref(null)

const origins = ref([])
const destinations = ref([])

const loadingOrigins = ref(false)
const loadingDestinations = ref(false)
const originsError = ref('')
const destinationsError = ref('')

function toSelectItems(airports) {
  return airports.map(a => ({ code: a.code, label: `${a.code} — ${a.name}` }))
}

onMounted(async () => {
  loadingOrigins.value = true
  originsError.value = ''
  try {
    const { data } = await fetchOrigins()
    origins.value = toSelectItems(data)
  } catch {
    originsError.value = 'Could not load airports. Please refresh the page.'
  } finally {
    loadingOrigins.value = false
  }
})

async function onOriginChange(code) {
  destination.value = null
  destinations.value = []
  destinationsError.value = ''
  searchResult.value = null

  if (!code) return

  loadingDestinations.value = true
  try {
    const { data } = await fetchDestinations(code)
    destinations.value = toSelectItems(data)
  } catch (err) {
    const msg = err.response?.data?.error
    destinationsError.value = msg ?? 'Could not load destinations. Please try again.'
  } finally {
    loadingDestinations.value = false
  }
}

function onSearch() {
  const originObj = origins.value.find(a => a.code === origin.value)
  const destObj = destinations.value.find(a => a.code === destination.value)
  searchResult.value = {
    tripType: tripType.value,
    origin: originObj,
    destination: destObj,
  }
}
</script>
