<script setup lang="ts">
import QrcodeVue from 'qrcode.vue';
import { ref } from 'vue';

const size :number = 300;
const textValue = ref("value")
</script>

<template>
  <h3>QR Generator</h3>
  <textarea v-model="textValue" class="form-control" id="value" rows="3"></textarea>
  <qrcode-vue :value="textValue" :size="size"></qrcode-vue>
</template>

<style scoped>

</style>
