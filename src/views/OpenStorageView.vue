<script setup lang="ts">
import { QrcodeStream } from 'vue-qrcode-reader'
import { ref } from 'vue'

const storageId = ref('');
const scanningResultList = ref([]);
const isScannerOpened = ref(false);
const scannerError = ref('');

function onDetect(detectedCodes:any) {
  scanningResultList.value = detectedCodes.map((code:any) => code.rawValue);
}
function onError(error:Error) {
    scannerError.value = error.message;
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
  <h3>Open Storage</h3>
  <div class="row">
    <div class="d-flex justify-content-center">
      <div class="input-group">
        <input v-model="storageId" type="text" class="form-control" placeholder="Enter storage ID">
        <button type="button" class="btn btn-outline-primary" data-bs-toggle="modal" data-bs-target="#exampleModal" @click="isScannerOpened = true">
          <i class="bi bi-qr-code-scan"></i> {{ $t('buttons.scanQR') }}
        </button>
        <RouterLink :to="'/storage/'+storageId" class="btn btn-primary">{{ $t('buttons.openStorage') }}</RouterLink>
      </div>
    </div>
	</div>
  <div class="modal fade" id="exampleModal" tabindex="-1" data-bs-backdrop="static" data-bs-keyboard="false" aria-hidden="true">
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <h1 class="modal-title fs-5" id="exampleModalLabel">{{ $t('labels.scanQR') }}</h1>
          <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close" @click="isScannerOpened = false"></button>
        </div>
        <div class="modal-body">
          <div v-if="scannerError" class="alert alert-danger" role="alert">
            <i class="bi bi-exclamation-triangle"></i>
            <div>{{ scannerError }}</div>
          </div>
          <qrcode-stream v-if="isScannerOpened" @detect="onDetect" @error="onError"></qrcode-stream>
          <button v-for="result in scanningResultList" type="button" class="btn btn-outline-primary" data-bs-dismiss="modal" @click="isScannerOpened = false; storageId = result">{{ result }}</button>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>

</style>
