<script setup lang="ts">
import { QrcodeStream } from 'vue-qrcode-reader'
import { ref } from 'vue'

const result = ref([''])

function onDetect(detectedCodes:any) {
  result.value = detectedCodes.map((code:any) => code.rawValue);
}
function onError(error:Error) {
    if (error.name === 'NotAllowedError') {
      // user denied camera access permission
    } else if (error.name === 'NotFoundError') {
      // no suitable camera device installed
    } else if (error.name === 'NotSupportedError') {
      // page is not served over HTTPS (or localhost)
    } else if (error.name === 'NotReadableError') {
      // maybe camera is already in use
    } else if (error.name === 'OverconstrainedError') {
      // did you request the front camera although there is none?
    } else if (error.name === 'StreamApiNotSupportedError') {
      // browser seems to be lacking features
    }
  }

</script>

<template>
  <h3>Scanner Page</h3>
  <qrcode-stream @detect="onDetect" @error="onError">
    <div class="d-grid gap-2">
      <RouterLink :to="'/item/'+item" class="btn btn-primary" v-for="item in result"> {{ item }} </RouterLink>
    </div>
  </qrcode-stream>
</template>

<style scoped>

</style>
