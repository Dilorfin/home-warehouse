<script setup lang="ts">
import QrcodeVue from 'qrcode.vue';
import { ref, watch } from 'vue';
import { useRoute } from 'vue-router'

const route = useRoute()

const itemId = ref(Array.isArray(route?.params?.id) ? route?.params?.id[0] : route?.params?.id);
const isLoading = ref(true);
const itemExists = ref(false);
const data = ref("");

fetch("/api/GetItem/?id=" + itemId.value).then(async (response: Response)=>{

  isLoading.value = false;
  itemExists.value = false;

  if (!response.bodyUsed)
  {
    //itemExists.value = true;
    //data.value = "Якісь данні";
    return;
  }

  let result = await response.json();
  if (response.ok)
  {
    itemExists.value = true;
    data.value = result["data"];
  }
  else
  {
    //data.value = result["error"];
  }
});
/* ???
watch(
  () => route.params.id,
  (newId, oldId) => {
    itemId.value = Array.isArray(newId) ? newId[0] : newId;
  }
);*/

</script>

<template>
  <div class="row">
    <div class="col-md-8 order-2 order-md-1">
      <div v-if="isLoading" class="spinner-border text-primary" role="status">
        <span class="visually-hidden">Loading...</span>
      </div>
      <div v-else-if="!itemExists" class="alert alert-primary" role="alert">
        No data found
        <a class="btn btn-primary">AAA</a>
      </div>
      <div v-else>
        {{ data }}
      </div>
    </div>
    <div class="col-md order-1 order-md-2">
      <div class="card">
        <div class="card-header">
          <h3 class="card-title">Item: {{ itemId }}</h3>
        </div>
        <div class="card-body">
          <div v-if="itemExists">
            <qrcode-vue :value="itemId" :size="100"></qrcode-vue>
          </div>
          <a class="btn btn-outline-primary">{{ $t('buttons.downloadQR') }}</a>
          <a class="btn btn-outline-primary">Print QR</a>
        </div>
        
      </div>
    </div>
  </div>
</template>

<style scoped>

</style>
