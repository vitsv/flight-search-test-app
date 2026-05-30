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

      <!-- Origin / Destination -->
      <v-row>
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

      <!-- Date pickers -->
      <v-row>
        <v-col cols="12" sm="6">
          <v-menu v-model="departureDateMenu" :close-on-content-click="false">
            <template #activator="{ props: menuProps }">
              <v-text-field
                v-bind="menuProps"
                :model-value="formattedDepartureDate"
                label="Departure date"
                prepend-inner-icon="mdi-calendar"
                readonly
                clearable
                @click:clear="onDepartureClear"
              />
            </template>
            <v-date-picker
              v-model="departureDate"
              :min="today"
              @update:model-value="departureDateMenu = false; returnDate = null"
            />
          </v-menu>
        </v-col>

        <v-col v-if="tripType === 'return'" cols="12" sm="6">
          <v-menu v-model="returnDateMenu" :close-on-content-click="false">
            <template #activator="{ props: menuProps }">
              <v-text-field
                v-bind="menuProps"
                :model-value="formattedReturnDate"
                label="Return date"
                prepend-inner-icon="mdi-calendar-arrow-right"
                readonly
                clearable
                :disabled="!departureDate"
                @click:clear="returnDate = null"
              />
            </template>
            <v-date-picker
              v-model="returnDate"
              :min="departureDate ?? today"
              @update:model-value="returnDateMenu = false"
            />
          </v-menu>
        </v-col>
      </v-row>

      <!-- Passengers -->
      <v-row align="center" class="mt-1 mb-3">
        <v-col cols="12" sm="6">
          <div class="text-body-2 text-medium-emphasis mb-2">Passengers</div>
          <div class="d-flex align-center">
            <v-btn
              icon="mdi-minus"
              variant="outlined"
              size="small"
              :disabled="passengers <= 1"
              @click="passengers--"
            />
            <span class="mx-4 text-h6 font-weight-medium">{{ passengers }}</span>
            <v-btn
              icon="mdi-plus"
              variant="outlined"
              size="small"
              :disabled="passengers >= 9"
              @click="passengers++"
            />
            <span class="ml-3 text-body-2 text-medium-emphasis">
              {{ passengers === 1 ? 'passenger' : 'passengers' }}
            </span>
          </div>
        </v-col>
      </v-row>

      <v-row>
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
import { ref, computed, watch, onMounted } from 'vue'
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

const departureDate = ref(null)
const returnDate = ref(null)
const departureDateMenu = ref(false)
const returnDateMenu = ref(false)
const passengers = ref(1)

const today = new Date()
today.setHours(0, 0, 0, 0)

function formatDate(date) {
  return date.toLocaleDateString('en-GB', { day: 'numeric', month: 'short', year: 'numeric' })
}

const formattedDepartureDate = computed(() =>
  departureDate.value ? formatDate(departureDate.value) : ''
)
const formattedReturnDate = computed(() =>
  returnDate.value ? formatDate(returnDate.value) : ''
)

watch(tripType, (type) => {
  if (type === 'one-way') returnDate.value = null
})

function toSelectItems(airports) {
  return airports.map(a => ({ code: a.code, label: `${a.code} — ${a.name}` }))
}

function onDepartureClear() {
  departureDate.value = null
  returnDate.value = null
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
    departureDate: departureDate.value,
    returnDate: tripType.value === 'return' ? returnDate.value : null,
    passengers: passengers.value,
  }
}
</script>
