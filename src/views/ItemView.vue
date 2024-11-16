<script setup lang="ts">
import QrcodeVue from 'qrcode.vue';
import { ref, watch } from 'vue';
import { useRoute } from 'vue-router'

const route = useRoute()

const size :number = 300;

const itemId = ref(Array.isArray(route?.params?.id) ? route?.params?.id[0] : route?.params?.id);
const isLoading = ref(true);
const itemExists = ref(false);
const data = ref("");

fetch("http://10.0.1.102:5000/get-data/" + itemId.value).then(async (response: Response)=>{
  console.log(response);
  let result = await response.json();
  if (response.ok)
  {
    itemExists.value = true;
    data.value = result["data"];
  }
  else
  {
    itemExists.value = false;
    data.value = result["error"];
  }
  isLoading.value = false;
});
/*watch(
  () => route.params.id,
  (newId, oldId) => {
    itemId.value = Array.isArray(newId) ? newId[0] : newId;
  }
);*/

</script>

<template>
  <h3>Item: {{ itemId }}</h3>
  <div v-if="isLoading" class="spinner-border text-primary" role="status">
    <span class="visually-hidden">Loading...</span>
  </div>
  <div v-if="itemExists">
    <p>{{ data }}</p>
    <qrcode-vue :value="itemId" :size="size"></qrcode-vue>
  </div>
  <div v-else-if="!itemExists">
    <p>{{ data }}</p>
    <qrcode-vue :value="itemId" :size="size"></qrcode-vue>
  </div>
</template>

<style scoped>

</style>
